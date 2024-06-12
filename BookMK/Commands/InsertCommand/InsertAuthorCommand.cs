using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.InsertFormViewModels;
using Polly;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.InsertCommand
{
    public class InsertAuthorCommand : AsyncCommandBase
    {
        private readonly AuthorFormViewModel vm;
        bool operationSucceeded = false;
        public InsertAuthorCommand(AuthorFormViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Int32 ID = vm.ID;
            String _Name = vm.Name;
            String _Note = vm.Note;

            if (string.IsNullOrWhiteSpace(_Name))
            {
                MessageBox.Show("Please enter an Author's name first!");
                return;
            }

            if (Author.IsExisted(_Name))
            {
                MessageBox.Show("This author already exists. Please check the name.");
                return;
            }

            Author newAuthor = new Author()
            {
                ID = ID,
                Name = _Name,
                Note = _Note
            };

            try
            {
                // Add to cache before attempting to insert
                SimpleCache.AddOrUpdate($"author_{ID}", newAuthor);

                // Define retry policy with exponential backoff
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            Log.Warning("Retry {RetryCount} of inserting author failed. Waiting {TimeSpan} before next retry. Exception: {Exception}", retryCount, timeSpan, exception);
                        });

                await retryPolicy.ExecuteAsync(async () =>
                {
                    DataProvider<Author> db = new DataProvider<Author>(Author.Collection);
                    await db.InsertOneAsync(newAuthor);
                    operationSucceeded = true;
                });

                if (operationSucceeded)
                {
                    MessageBox.Show("Added a new author!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Window f = parameter as Window;
                    f?.Close();

                    // Remove from cache after successful insert
                    SimpleCache.Remove($"author_{ID}");

                    // Log success
                    Log.Information("New author added: {AuthorName}", _Name);
                }
                else
                {
                    // Notify user after all retries failed
                    MessageBox.Show("Failed to add the author after multiple attempts. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Retrieve from cache for further action if needed
                var cachedAuthor = SimpleCache.Get<Author>($"author_{ID}");

                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while inserting a new author. Cached author: {@Author}", cachedAuthor);

               
               
            }
        }
    }
}
