using BookMK.Models;
using BookMK.ViewModels;
using MongoDB.Driver;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands
{
    public class ChangePasswordCommand : AsyncCommandBase
    {
        private readonly SettingViewModel _vm;
        private static readonly ILogger _logger = Log.ForContext(typeof(ChangePasswordCommand));

        public ChangePasswordCommand(SettingViewModel vm, int kind)
        {
            _vm = vm;
            _logger.Information("ChangePasswordCommand instantiated.");
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(_vm.NewPassword))
                {
                    MessageBox.Show("Your new password is invalid!!!");
                    _logger.Warning("Invalid new password provided.");
                    return;
                }

                MessageBoxResult asking = MessageBox.Show("Are you sure to save change?", "REMINDING",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (asking == MessageBoxResult.No)
                {
                    _logger.Information("User cancelled password change.");
                    return;
                }

                string newPasswordHash = Staff.HashPassword(_vm.NewPassword);

                await Task.Run(() =>
                {
                    DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                    FilterDefinition<Staff> filter = Builders<Staff>.Filter.Eq("_id", _vm.CurrentStaff.ID);
                    UpdateDefinition<Staff> update = Builders<Staff>.Update.Set(x => x.PasswordHash, newPasswordHash);
                    db.Update(filter, update);

                    MessageBox.Show("Your password has been updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    _logger.Information("Password updated successfully.");
                    Window f = parameter as Window;
                    f?.Close();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Error(ex, "An error occurred while changing the password.");
            }
        }
    }
}
