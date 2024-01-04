using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.DeleteCommand
{
    public class DeleteBookCommand:AsyncCommandBase
    {
        private readonly ViewBookViewModel vm;
        private readonly StringBuilder filename;
        public DeleteBookCommand(ViewBookViewModel vm,StringBuilder F )
        {
            this.vm = vm;
            this.filename = F;
        }


        public override async Task ExecuteAsync(object parameter)
        {
            Book _CurrentBook = vm.CurrentBook;
            string filepath = filename.ToString();


            MessageBoxResult asking = MessageBox.Show("Are you sure to DELETE?", "REMINDING",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (asking == MessageBoxResult.No) return;
            try
            {

                await Task.Run(() =>
                {
                    FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.ID, _CurrentBook.ID);
                    DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
                    db.Delete(filter);



                    if (!String.IsNullOrEmpty(filepath))
                    {
                        ImageStorage.DeleteImage(ImageStorage.BookImageLocation, filepath);
                    }
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Book deleted !", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window f = parameter as Window;
                        f?.Close();
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
