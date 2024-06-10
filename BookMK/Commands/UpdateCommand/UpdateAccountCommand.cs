using BookMK.Models;
using BookMK.ViewModels;
using MongoDB.Driver;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateAccountCommand : AsyncCommandBase
    {
        private readonly SettingViewModel vm;

        public UpdateAccountCommand(SettingViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Staff a = vm.CurrentStaff;

            try
            {
                await Task.Run(() =>
                {
                    FilterDefinition<Staff> filter = Builders<Staff>.Filter.Eq(x => x.ID, a.ID);
                    UpdateDefinition<Staff> update = Builders<Staff>.Update
                        .Set(x => x.Email, vm.Email)
                        .Set(x => x.IsVerified, true);

                    DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                    db.Update(filter, update);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Email verified successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window f = parameter as Window;
                        f?.Close();
                    });

                    // Log success
                    Log.Information("Account updated successfully: ID - {StaffID}, Email - {Email}", a.ID, vm.Email);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while updating account: ID - {StaffID}, Email - {Email}", a.ID, vm.Email);
            }
        }
    }
}
