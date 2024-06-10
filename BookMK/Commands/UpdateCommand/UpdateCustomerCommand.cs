using BookMK.Models;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateCustomerCommand : AsyncCommandBase
    {
        private ViewCustomerViewModel vm;

        public UpdateCustomerCommand(ViewCustomerViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Customer updatedCustomer = vm.CurrentCustomer;

            try
            {
                await Task.Run(() =>
                {
                    FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(x => x.ID, updatedCustomer.ID);
                    UpdateDefinition<Customer> update = Builders<Customer>.Update
                        .Set(x => x.FullName, updatedCustomer.FullName)
                        .Set(x => x.Address, updatedCustomer.Address)
                        .Set(x => x.Phone, updatedCustomer.Phone);
                    // Add more attributes to update if needed

                    DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
                    db.Update(filter, update);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window f = parameter as Window;
                        f?.Close();
                    });

                    // Log success
                    Log.Information("Customer updated successfully: ID - {CustomerID}, FullName - {FullName}, Address - {Address}, Phone - {Phone}", updatedCustomer.ID, updatedCustomer.FullName, updatedCustomer.Address, updatedCustomer.Phone);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while updating customer: ID - {CustomerID}, FullName - {FullName}, Address - {Address}, Phone - {Phone}", updatedCustomer.ID, updatedCustomer.FullName, updatedCustomer.Address, updatedCustomer.Phone);
            }
        }
    }
}
