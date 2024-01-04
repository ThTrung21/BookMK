using BookMK.Models;
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
    public class DeleteDiscountCommand: AsyncCommandBase
    {
        private readonly ViewDiscountViewModel vm;
        public DeleteDiscountCommand(ViewDiscountViewModel vm)
        {
            this.vm = vm;
        }
        public override async Task ExecuteAsync(object parameter)
        {
            Discount _CurrentDiscount = vm.CurrentDiscount;

            MessageBoxResult asking = MessageBox.Show("Are you sure to DELETE?", "REMINDING",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (asking == MessageBoxResult.No) return;


            try
            {
                await Task.Run(() =>
                {
                    FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq(x => x.ID, _CurrentDiscount.ID);
                    DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
                    db.Delete(filter);



                  
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
