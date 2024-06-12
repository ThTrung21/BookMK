using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.InsertFormViewModels;
using MongoDB.Driver;
using Polly;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.InsertCommand
{
    public class InsertBookCommand : AsyncCommandBase
    {
        private readonly InsertBookViewModel vm;
        private readonly StringBuilder filename;
        bool operationSucceeded = false;
        public InsertBookCommand(InsertBookViewModel vm, StringBuilder filename)
        {
            this.vm = vm;
            this.filename = filename;
        }

        public int GetAuthorID(Author auth)
        {
            string authName = auth.Name;
            DataProvider<Author> db = new DataProvider<Author>(Author.Collection);
            FilterDefinition<Author> filter = Builders<Author>.Filter.Where(a => a.Name.ToLower().Trim() == authName);
            List<Author> authors = db.ReadFiltered(filter);

            // If an author with the specified name is found, return the AuthorID; otherwise, return -1
            return authors.Count > 0 ? authors[0].ID : -1;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (vm.SelectedAuthor == null || vm.Title == null || vm.ReleaseYear == null || vm.SellPrice == 0)
            {
                MessageBox.Show("Please fill in every field!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Int32 _ID = vm.ID;
            String _Title = vm.Title;
            String _Author = vm.SelectedAuthor.Name;
            List<string> _Genre = vm.SelectedGenres;
            String _ReleaseYear = vm.ReleaseYear;
            Double _SellPrice = vm.SellPrice;

            Book newBook = new Book()
            {
                ID = (int)_ID,
                Cover = _ID + ".png",
                SellPrice = _SellPrice,
                ReleaseYear = _ReleaseYear,
                Genre = _Genre,
                Title = _Title,
                AuthorName = _Author,
                Stock = 0
            };

            try
            {
                

                if (string.IsNullOrWhiteSpace(_Title) || string.IsNullOrWhiteSpace(_Author) || string.IsNullOrWhiteSpace(_ReleaseYear) || _SellPrice == 0)
                {
                    MessageBox.Show("Please fill in every field!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Book.IsExisted(_Title, _Author, _ReleaseYear))
                {
                    MessageBox.Show("This book already exists!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

               

                // Add to cache before attempting to insert
                SimpleCache.AddOrUpdate($"book_{_ID}", newBook);

                // Define retry policy with exponential backoff
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            Log.Warning("Retry {RetryCount} of inserting book failed. Waiting {TimeSpan} before next retry. Exception: {Exception}", retryCount, timeSpan, exception);
                        });

                await retryPolicy.ExecuteAsync(async () =>
                {
                    DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
                    await db.InsertOneAsync(newBook);
                    operationSucceeded = true;
                });

                // Store image if the file exists
                string filepath = filename.ToString();
                if (!String.IsNullOrEmpty(filepath) && File.Exists(filepath))
                {
                    ImageStorage.StoreImage(filepath, ImageStorage.BookImageLocation, newBook.Cover);
                }

                if (operationSucceeded)
                {
                    MessageBox.Show("Added a new book!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Window f = parameter as Window;
                    f?.Close();

                    // Remove from cache after successful insert
                    SimpleCache.Remove($"book_{_ID}");

                    // Log success
                    Log.Information("New book added: {BookTitle} by {BookAuthor}", _Title, _Author);
                }
                else
                {
                    // Notify user after all retries failed
                    MessageBox.Show("Failed to add the book after multiple attempts. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Retrieve from cache for further action if needed
                var cachedBook = SimpleCache.Get<Book>($"book_{_ID}");

                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while inserting a new book. Cached book: {@Book}", cachedBook);

               
            }
        }
    }
}
