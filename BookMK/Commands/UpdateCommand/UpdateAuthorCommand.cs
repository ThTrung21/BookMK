using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.InsertFormViewModels;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using Polly;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateAuthorCommand : AsyncCommandBase
    {
        private ViewAuthorViewModel vm;
        bool operationSucceeded = false;

        public UpdateAuthorCommand(ViewAuthorViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Author updatedAuthor = vm.CurrentAuthor;

            try
            {
                // Check if both Name and Note are empty
                if (string.IsNullOrEmpty(updatedAuthor.Name) && string.IsNullOrEmpty(updatedAuthor.Note))
                {
                    MessageBox.Show("Both Name and Note are empty. No update performed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Add to cache before attempting to update
                SimpleCache.AddOrUpdate($"author_{updatedAuthor.ID}", updatedAuthor);

                // Define retry policy with exponential backoff
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            Log.Warning("Retry {RetryCount} of updating author failed. Waiting {TimeSpan} before next retry. Exception: {Exception}", retryCount, timeSpan, exception);
                        });

                await retryPolicy.ExecuteAsync(async () =>
                {
                    FilterDefinition<Author> filter = Builders<Author>.Filter.Eq(x => x.ID, updatedAuthor.ID);
                    UpdateDefinition<Author> update = Builders<Author>.Update
                        .Set(x => x.Name, updatedAuthor.Name)
                        .Set(x => x.Note, updatedAuthor.Note);

                    DataProvider<Author> db = new DataProvider<Author>(Author.Collection);
                    await db.UpdateAsync(filter, update);
                    operationSucceeded = true;
                });

                if (operationSucceeded)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Author updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window f = parameter as Window;
                        f?.Close();
                    });

                    // Remove from cache after successful update
                    SimpleCache.Remove($"author_{updatedAuthor.ID}");

                    // Log success
                    Log.Information("Author updated successfully: ID - {AuthorID}, Name - {Name}, Note - {Note}", updatedAuthor.ID, updatedAuthor.Name, updatedAuthor.Note);
                }
                else
                {
                    // Notify user after all retries failed
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Failed to update the author after multiple attempts. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            }
            catch (Exception ex)
            {
                // Retrieve from cache for further action if needed
                var cachedAuthor = SimpleCache.Get<Author>($"author_{updatedAuthor.ID}");

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });

                // Log error
                Log.Error(ex, "Error occurred while updating author. Cached author: {@Author}", cachedAuthor);
            }
        }
    }
}
