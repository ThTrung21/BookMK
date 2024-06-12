using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using Polly;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateCustomerCommand : AsyncCommandBase
    {
        private readonly ViewCustomerViewModel vm;
        private bool operationSucceeded = false;

        public UpdateCustomerCommand(ViewCustomerViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Customer updatedCustomer = vm.CurrentCustomer;

            try
            {
                // Add to cache before attempting to update
                SimpleCache.AddOrUpdate($"customer_{updatedCustomer.ID}", updatedCustomer);

                // Define retry policy with exponential backoff
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            Log.Warning("Retry {RetryCount} of updating customer failed. Waiting {TimeSpan} before next retry. Exception: {Exception}", retryCount, timeSpan, exception);
                        });

                await retryPolicy.ExecuteAsync(async () =>
                {
                    FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(x => x.ID, updatedCustomer.ID);
                    UpdateDefinition<Customer> update = Builders<Customer>.Update
                        .Set(x => x.FullName, updatedCustomer.FullName)
                        .Set(x => x.Address, updatedCustomer.Address)
                        .Set(x => x.Phone, updatedCustomer.Phone);
                    // Add more attributes to update if needed

                    DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
                    await db.UpdateAsync(filter, update);
                    operationSucceeded = true;
                });

                if (operationSucceeded)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window f = parameter as Window;
                        f?.Close();
                    });

                    // Remove from cache after successful update
                    SimpleCache.Remove($"customer_{updatedCustomer.ID}");

                    // Log success
                    Log.Information("Customer updated successfully: ID - {CustomerID}, FullName - {FullName}, Address - {Address}, Phone - {Phone}", updatedCustomer.ID, updatedCustomer.FullName, updatedCustomer.Address, updatedCustomer.Phone);
                }
                else
                {
                    // Notify user after all retries failed
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Failed to update the customer after multiple attempts. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            }
            catch (Exception ex)
            {
                // Retrieve from cache for further action if needed
                var cachedCustomer = SimpleCache.Get<Customer>($"customer_{updatedCustomer.ID}");

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });

                // Log error
                Log.Error(ex, "Error occurred while updating customer. Cached customer: {@Customer}", cachedCustomer);
            }
        }
    }
}
