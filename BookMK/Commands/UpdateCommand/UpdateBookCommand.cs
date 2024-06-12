using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.InsertFormViewModels;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using Polly;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateBookCommand : AsyncCommandBase
    {
        private readonly ViewBookViewModel vm;
        private readonly StringBuilder filename;
        private bool operationSucceeded = false;

        public UpdateBookCommand(ViewBookViewModel vm, StringBuilder filename)
        {
            this.vm = vm;
            this.filename = filename;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Book _CurrentBook = vm.CurrentBook;

            if (_CurrentBook.SellPrice == 0)
            {
                MessageBox.Show("Please change the price!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Add to cache before attempting to update
                SimpleCache.AddOrUpdate($"book_{_CurrentBook.ID}", _CurrentBook);

                // Define retry policy with exponential backoff
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            Log.Warning("Retry {RetryCount} of updating book failed. Waiting {TimeSpan} before next retry. Exception: {Exception}", retryCount, timeSpan, exception);
                        });

                await retryPolicy.ExecuteAsync(async () =>
                {
                    FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.ID, _CurrentBook.ID);
                    UpdateDefinition<Book> update = Builders<Book>.Update
                        .Set(x => x.SellPrice, _CurrentBook.SellPrice);

                    // More attributes to update can be added here

                    DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
                    await db.UpdateAsync(filter, update);
                    operationSucceeded = true;
                });

                if (operationSucceeded)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Book updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window f = parameter as Window;
                        f?.Close();
                    });

                    // Remove from cache after successful update
                    SimpleCache.Remove($"book_{_CurrentBook.ID}");

                    // Log success
                    Log.Information("Book updated successfully: ID - {BookID}, Title - {Title}, New Sell Price - {SellPrice}", _CurrentBook.ID, _CurrentBook.Title, _CurrentBook.SellPrice);
                }
                else
                {
                    // Notify user after all retries failed
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Failed to update the book after multiple attempts. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            }
            catch (Exception ex)
            {
                // Retrieve from cache for further action if needed
                var cachedBook = SimpleCache.Get<Book>($"book_{_CurrentBook.ID}");

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });

                // Log error
                Log.Error(ex, "Error occurred while updating book. Cached book: {@Book}", cachedBook);
            }
        }
    }
}
