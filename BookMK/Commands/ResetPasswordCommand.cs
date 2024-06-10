using BookMK.Models;
using BookMK.ViewModels;
using MongoDB.Driver;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands
{
    public class ResetPasswordCommand : AsyncCommandBase
    {
        private readonly ResetPasswordViewModel _viewModel;
        private readonly string _newPassword;
        private static readonly ILogger _logger = Log.ForContext(typeof(ResetPasswordCommand));

        public ResetPasswordCommand(ResetPasswordViewModel viewModel, string newPassword)
        {
            _viewModel = viewModel;
            _newPassword = newPassword;
            _logger.Information("ResetPasswordCommand instantiated.");
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                string newPasswordHash = Staff.HashPassword(_newPassword);

                await Task.Run(() =>
                {
                    DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                    FilterDefinition<Staff> filter = Builders<Staff>.Filter.Eq(x => x.Email, _viewModel.Email);
                    UpdateDefinition<Staff> update = Builders<Staff>.Update.Set(x => x.PasswordHash, newPasswordHash);
                    db.Update(filter, update);

                    _logger.Information("Password reset successfully for email: {Email}", _viewModel.Email);
                    Window f = parameter as Window;
                    f?.Close();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Error(ex, "An error occurred while resetting the password.");
            }
        }
    }
}
