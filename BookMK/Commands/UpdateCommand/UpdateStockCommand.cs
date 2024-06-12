using BookMK.Models;
using MongoDB.Driver;
using Serilog;
using System;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    internal class UpdateStockCommand : CommandBase
    {
        private readonly OrderItem b;

        public UpdateStockCommand(OrderItem b)
        {
            this.b = b;
        }

        public override void Execute(object parameter)
        {
            try
            {
                Book currentBook = Book.GetBook(b.BookID);
                if (currentBook == null)
                {
                    MessageBox.Show($"An error occurred", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.ID, currentBook.ID);
                UpdateDefinition<Book> update = Builders<Book>.Update
                    .Set(x => x.Stock, currentBook.Stock); 
                DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
                db.Update(filter, update);

                // Log success
                Log.Information("Stock updated successfully for Book ID {BookID}", currentBook.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while updating stock for Book ID {BookID}", b.BookID);
            }
        }
    }
}
