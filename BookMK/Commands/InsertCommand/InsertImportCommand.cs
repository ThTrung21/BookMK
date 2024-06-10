using BookMK.Models;
using BookMK.ViewModels.InsertFormViewModels;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.InsertCommand
{
    public class InsertImportCommand : AsyncCommandBase
    {
        private readonly InsertImportViewModel vm;

        public InsertImportCommand(InsertImportViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                int _ID = vm.ID;
                ObservableCollection<ImportItem> list = vm.ImportItemList;
                if (list.Count() < 1)
                {
                    MessageBox.Show("There's no item in your import list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Initialize the import
                Import i = new Import()
                {
                    ID = _ID,
                    Items = vm.ImportItemList,
                    Time = DateTime.Now,
                    TotalPrice = vm.TotalPrice
                };

                // Update books info
                foreach (var item in vm.ImportItemList)
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

                DataProvider<Import> db = new DataProvider<Import>(Import.Collection);
                await db.InsertOneAsync(i);

                MessageBox.Show("A new import has been recorded!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Window f = parameter as Window;
                f?.Close();

                // Log success
                Log.Information("A new import has been recorded: ImportID - {ImportID}, Time - {ImportTime}", _ID, DateTime.Now);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while inserting a new import.");
            }
        }
    }
}
