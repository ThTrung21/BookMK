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
    public class UpdateStaffCommand : AsyncCommandBase
    {
        private readonly ViewStaffViewModel vm;
        private bool operationSucceeded = false;

        public UpdateStaffCommand(ViewStaffViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Staff updatedStaff = vm.CurrentStaff;

            try
            {
                // Add to cache before attempting to update
                SimpleCache.AddOrUpdate($"staff_{updatedStaff.ID}", updatedStaff);

                // Define retry policy with exponential backoff
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            Log.Warning("Retry {RetryCount} of updating staff failed. Waiting {TimeSpan} before next retry. Exception: {Exception}", retryCount, timeSpan, exception);
                        });

                await retryPolicy.ExecuteAsync(async () =>
                {
                    FilterDefinition<Staff> filter = Builders<Staff>.Filter.Eq(x => x.ID, updatedStaff.ID);
                    UpdateDefinition<Staff> update = Builders<Staff>.Update
                        .Set(x => x.FullName, updatedStaff.FullName)
                        .Set(x => x.Username, updatedStaff.Username)
                        .Set(x => x.Phone, updatedStaff.Phone);
                    // Add more attributes to update if needed

                    DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                    await db.UpdateAsync(filter, update);
                    operationSucceeded = true;
                });

                if (operationSucceeded)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Staff updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window f = parameter as Window;
                        f?.Close();
                    });

                    // Remove from cache after successful update
                    SimpleCache.Remove($"staff_{updatedStaff.ID}");

                    // Log success
                    Log.Information("Staff updated successfully: ID - {StaffID}, FullName - {FullName}, Username - {Username}, Phone - {Phone}", updatedStaff.ID, updatedStaff.FullName, updatedStaff.Username, updatedStaff.Phone);
                }
                else
                {
                    // Notify user after all retries failed
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Failed to update the staff after multiple attempts. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            }
            catch (Exception ex)
            {
                // Retrieve from cache for further action if needed
                var cachedStaff = SimpleCache.Get<Staff>($"staff_{updatedStaff.ID}");

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });

                // Log error
                Log.Error(ex, "Error occurred while updating staff. Cached staff: {@Staff}", cachedStaff);
            }
        }
    }
}
