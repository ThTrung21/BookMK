using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using Serilog;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.DeleteCommand
{
    public class DeleteBookCommand : AsyncCommandBase
    {
        private readonly ViewBookViewModel _vm;
        private readonly StringBuilder _filename;
        private static readonly ILogger _logger = Log.ForContext(typeof(DeleteBookCommand));

        public DeleteBookCommand(ViewBookViewModel vm, StringBuilder filename)
        {
            _vm = vm;
            _filename = filename;
            _logger.Information("DeleteBookCommand instantiated.");
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Book currentBook = _vm.CurrentBook;
            string filepath = _filename.ToString();

            MessageBoxResult asking = MessageBox.Show("Are you sure you want to DELETE?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (asking == MessageBoxResult.No) return;

            try
            {
                await Task.Run(() =>
                {
                    FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.ID, currentBook.ID);
                    DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
                    db.Delete(filter);

                    if (!string.IsNullOrEmpty(filepath))
                    {
                        ImageStorage.DeleteImage(ImageStorage.BookImageLocation, filepath);
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Book deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window f = parameter as Window;
                        f?.Close();
                    });
                });

                _logger.Information("Book deleted: {BookId}", currentBook.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Error(ex, "An error occurred while deleting the book.");
            }
        }
    }
}
