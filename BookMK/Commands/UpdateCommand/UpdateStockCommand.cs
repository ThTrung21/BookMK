using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    internal class UpdateStockCommand:CommandBase
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
                Book CurrentBook = Book.GetBook(b.BookID);
                if (CurrentBook == null)
                {
                    MessageBox.Show($"An error occurred", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return ;
                }
                if (CurrentBook == null)
                {
                    MessageBox.Show($"An error occurred", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.ID, CurrentBook.ID);
                    UpdateDefinition<Book> update = Builders<Book>.Update

                        .Set(x => x.Stock, CurrentBook.SellPrice);

                    //more attributes

                    DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
                    db.Update(filter, update);



                 
                   
              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
