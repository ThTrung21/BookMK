using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.InsertFormViewModels;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateBookCommand : AsyncCommandBase
    {
        private readonly ViewBookViewModel vm;
        private readonly StringBuilder filename;
        private DataProvider<Author> authorDataProvider;

        public UpdateBookCommand(ViewBookViewModel vm, StringBuilder filename)
        {
            this.vm = vm;

            this.filename = filename;

        }
        

        public override async Task ExecuteAsync(object parameter)
        {
            Book _CurrentBook = vm.CurrentBook;
            string filepath = filename.ToString();


            if ( _CurrentBook.SellPrice == 0)
            {
                MessageBox.Show("Please change the price!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {

                await Task.Run(() =>
                {
                    FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.ID, _CurrentBook.ID);
                    UpdateDefinition<Book> update = Builders<Book>.Update
                       
                        .Set(x => x.SellPrice, _CurrentBook.SellPrice);
                        
                    //more attributes

                    DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
                    db.Update(filter, update);


                    
                    if (!String.IsNullOrEmpty(filepath))
                    {
                        ImageStorage.StoreImage(filepath, ImageStorage.BookImageLocation, _CurrentBook.Cover);
                    }
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Book updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
