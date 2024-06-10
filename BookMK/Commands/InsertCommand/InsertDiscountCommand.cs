using BookMK.Models;
using BookMK.ViewModels.InsertFormViewModels;
using MongoDB.Driver;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.InsertCommand
{
    public class InsertDiscountCommand : AsyncCommandBase
    {
        private readonly InsertDiscountViewModel vm;
        private int discounttype;

        public InsertDiscountCommand(InsertDiscountViewModel vm, int discounttype)
        {
            this.vm = vm;
            this.discounttype = discounttype;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                int _ID = vm.ID;
                DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);

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

                        Discount d = new Discount()
                        {
                            ID = _ID,
                            Type = "Percentage",
                            BookID = bookdiscount,
                            BookID_free = -1,
                            EligibleBill = 0,
                            Value = vm.Value,
                            Time = DateTime.Now,
                        };

                        if (!Discount.IsExistedPercentageAll() || !vm.IsChecked)
                            await db.InsertOneAsync(d);
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
                        Discount d1 = new Discount()
                        {
                            ID = _ID,
                            Type = "Amount",
                            BookID = 0,
                            BookID_free = -1,
                            EligibleBill = vm.EligibleBill,
                            Value = vm.Value,
                            Time = DateTime.Now,
                        };
                        await db.InsertOneAsync(d1);
                        break;

                    case 3:
                        if (vm.SelectedBaseBooks == null || vm.SelectedFreeBook == null)
                        {
                            MessageBox.Show("Please check your inputs!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        Discount d2 = new Discount()
                        {
                            ID = _ID,
                            Type = "BOGO",
                            BookID = vm.SelectedBaseBooks.ID,
                            BookID_free = vm.SelectedFreeBook.ID,
                            EligibleBill = 0,
                            Value = 0,
                            Time = DateTime.Now,
                        };
                        await db.InsertOneAsync(d2);
                        break;

                }

                MessageBox.Show("A new discount has been created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Window f = parameter as Window;
                f?.Close();

                // Log success
                Log.Information("A new discount has been created: DiscountID - {DiscountID}, Type - {DiscountType}", _ID, discounttype);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while inserting a new discount.");
            }
        }
    }
}
