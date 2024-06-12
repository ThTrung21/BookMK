using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels;
using MongoDB.Driver;
using Polly;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateAccountCommand : AsyncCommandBase
    {
        private readonly SettingViewModel vm;
        private bool operationSucceeded = false;

        public UpdateAccountCommand(SettingViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Staff a = vm.CurrentStaff;
            int ID = a.ID;
            try
            {
                // Add to cache before attempting to update
                SimpleCache.AddOrUpdate($"staff_{ID}", a);

                // Define retry policy with exponential backoff
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            Log.Warning("Retry {RetryCount} of updating user failed. Waiting {TimeSpan} before next retry. Exception: {Exception}", retryCount, timeSpan, exception);
                        });

                await retryPolicy.ExecuteAsync(async () =>
                {
                    FilterDefinition<Staff> filter = Builders<Staff>.Filter.Eq(x => x.ID, a.ID);
                    UpdateDefinition<Staff> update = Builders<Staff>.Update
                        .Set(x => x.Email, vm.Email)
                        .Set(x => x.IsVerified, true);

                    DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                    await db.UpdateAsync(filter, update);
                    operationSucceeded = true;
                });

                if (operationSucceeded)
                {
                    MessageBox.Show("Email verified successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Window f = parameter as Window;
                    f?.Close();

                    // Remove from cache after successful update
                    SimpleCache.Remove($"staff_{ID}");

                    // Log success
                    Log.Information("Account updated successfully: ID - {StaffID}, Email - {Email}", a.ID, vm.Email);
                }
                else
                {
                    MessageBox.Show("Failed to update the account after multiple attempts. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                var cachedStaff = SimpleCache.Get<Staff>($"staff_{ID}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while updating account: ID - {StaffID}, Email - {Email}", a.ID, vm.Email);
            }
        }
    }
}
