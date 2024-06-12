using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.InsertFormViewModels;
using MongoDB.Driver;
using Polly;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.InsertCommand
{
    public class InsertDiscountCommand : AsyncCommandBase
    {
        private readonly InsertDiscountViewModel vm;
        private readonly int discounttype;
        bool operationSucceeded=false;
        public InsertDiscountCommand(InsertDiscountViewModel vm, int discounttype)
        {
            this.vm = vm;
            this.discounttype = discounttype;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            int _ID = vm.ID;

            // Create the discount object outside the try block to ensure availability for logging in the catch block
            Discount newDiscount = null;
            
            try
            {
                DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
                var retryPolicy = Policy
                            .Handle<Exception>()
                            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                                (exception, timeSpan, retryCount, context) =>
                                {
                                    Log.Warning("Retry {RetryCount} of inserting discount failed. Waiting {TimeSpan} before next retry. Exception: {Exception}", retryCount, timeSpan, exception);
                                });
                switch (discounttype)
                {
                    case 1:
                        int bookdiscount;
                        if (vm.IsChecked)
                        {
                            bookdiscount = 0;
                        }
                        else
                        {
                            if (vm.SelectedBaseBooks == null)
                            {
                                MessageBox.Show("Please select a book first", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                            bookdiscount = vm.SelectedBaseBooks.ID;
                        }

                        newDiscount = new Discount()
                        {
                            ID = _ID,
                            Type = "Percentage",
                            BookID = bookdiscount,
                            BookID_free = -1,
                            EligibleBill = 0,
                            Value = vm.Value,
                            Time = DateTime.Now,
                        };
                        SimpleCache.AddOrUpdate($"discount_{_ID}", newDiscount);

                        // Define retry policy with exponential backoff
                        

                        if (!Discount.IsExistedPercentageAll() || !vm.IsChecked)
                            await retryPolicy.ExecuteAsync(async () =>
                            {
                                await db.InsertOneAsync(newDiscount); operationSucceeded=true;
                            });
                        else
                        {
                            MessageBox.Show("Store-wide discount already existed \n Discount will not be added!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        break;

                    case 2:
                        if (vm.Value == 0)
                        {
                            MessageBox.Show("Please check the discount value", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        newDiscount = new Discount()
                        {
                            ID = _ID,
                            Type = "Amount",
                            BookID = 0,
                            BookID_free = -1,
                            EligibleBill = vm.EligibleBill,
                            Value = vm.Value,
                            Time = DateTime.Now,
                        };
                        SimpleCache.AddOrUpdate($"discount_{_ID}", newDiscount);

                        // Define retry policy with exponential backoff
                         

                        await retryPolicy.ExecuteAsync(async () =>
                        {
                            await db.InsertOneAsync(newDiscount); operationSucceeded=true;
                        });
                        break;

                    case 3:
                        if (vm.SelectedBaseBooks == null || vm.SelectedFreeBook == null)
                        {
                            MessageBox.Show("Please check your inputs!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        newDiscount = new Discount()
                        {
                            ID = _ID,
                            Type = "BOGO",
                            BookID = vm.SelectedBaseBooks.ID,
                            BookID_free = vm.SelectedFreeBook.ID,
                            EligibleBill = 0,
                            Value = 0,
                            Time = DateTime.Now,
                        };
                        SimpleCache.AddOrUpdate($"discount_{_ID}", newDiscount);

                        await retryPolicy.ExecuteAsync(async () =>
                        {
                            await db.InsertOneAsync(newDiscount); operationSucceeded = true;
                        });
                        break;

                }
                if (operationSucceeded)
                {
                    MessageBox.Show("A new discount has been created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Window f = parameter as Window;
                f?.Close();
                // Remove from cache after successful insert
                SimpleCache.Remove($"discount_{_ID}");
                // Log success
                Log.Information("A new discount has been created: DiscountID - {DiscountID}, Type - {DiscountType}", _ID, discounttype);
                }
                else
                {
                    // Notify user after all retries failed
                    MessageBox.Show("Failed to create the discount after multiple attempts. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Retrieve from cache for further action if needed
                var cachedDiscount = SimpleCache.Get<Discount>($"discount_{_ID}");

                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while inserting a new discount. Cached discount: {@Discount}", cachedDiscount);

                
            }
        }
    }
}
