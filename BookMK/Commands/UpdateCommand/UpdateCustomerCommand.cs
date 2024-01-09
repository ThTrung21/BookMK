using BookMK.Models;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateCustomerCommand: AsyncCommandBase
    {
        ViewCustomerViewModel vm;

        public UpdateCustomerCommand(ViewCustomerViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Customer updatedCustomer = vm.CurrentCustomer;

            try
            {
                // Check if both Name and Note are empty
                

                await Task.Run(() =>
                {
                    FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(x => x.ID, updatedCustomer.ID);
                    UpdateDefinition<Customer> update = Builders<Customer>.Update
                        .Set(x => x.FullName, updatedCustomer.FullName)
                        
                        .Set(x => x.Address, updatedCustomer.Address)
                        .Set(x => x.Phone, updatedCustomer.Phone);
                        //more attributes

                    DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
                    db.Update(filter, update);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
