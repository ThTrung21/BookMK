using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.InsertFormViewModels;
using MongoDB.Driver;
using Polly;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.InsertCommand
{
    public class InsertImportCommand : AsyncCommandBase
    {
        private readonly InsertImportViewModel vm;
        bool operationSucceeded = false;
        public InsertImportCommand(InsertImportViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            // Initialize the import
            int _ID = vm.ID;
            Import i = new Import()
            {
                ID = _ID,
                Items = vm.ImportItemList,
                Time = DateTime.Now,
                TotalPrice = vm.TotalPrice
            };
            try
            {
                

                ObservableCollection<ImportItem> list = vm.ImportItemList;
                if (list.Count() < 1)
                {
                    MessageBox.Show("There's no item in your import list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }



                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            Log.Warning("Retry {RetryCount} of update user failed. Waiting {TimeSpan} before next retry. Exception: {Exception}", retryCount, timeSpan, exception);
                        });

                // Update books info
                foreach (var item in vm.ImportItemList)
                {
                    try
                    {
                       

                        string currentbook = item.ImportBook.ToString();
                        int currentid = item.BookID;
                        int amount = item.Amount;
                        Book target = Book.GetBook(currentid);
                       

                        if (target.Stock + amount > 200)
                        {
                            MessageBox.Show($"{target.Title} still has too much in stock. Cannot add to import.", "Stock Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        


                        FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.ID, currentid);
                        UpdateDefinition<Book> update = Builders<Book>.Update.Inc(x => x.Stock, amount);
                        
                            DataProvider<Book> importbookdb = new DataProvider<Book>(Book.Collection);
                        importbookdb.Update(filter, update);
                        
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                await retryPolicy.ExecuteAsync(async () =>
                {
                    DataProvider<Import> db = new DataProvider<Import>(Import.Collection);
                    await db.InsertOneAsync(i);  operationSucceeded = true;
                });


                if (operationSucceeded)
                {
                    MessageBox.Show("A new import has been recorded!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Window f = parameter as Window;
                f?.Close();


                // Remove from cache after successful insert
                SimpleCache.Remove($"import_ {_ID}");

                // Log success
                Log.Information("A new import has been recorded: ImportID - {ImportID}, Time - {ImportTime}", _ID, DateTime.Now);
                }
                else
                {
                    // Notify user after all retries failed
                    MessageBox.Show("Failed to create the import after multiple attempts. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Retrieve from cache for further action if needed
                var cachedAuthor = SimpleCache.Get<Import>($"import_{_ID}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while inserting a new import.");
            }
        }
    }
}
