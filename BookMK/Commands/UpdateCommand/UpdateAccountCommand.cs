using BookMK.Models;
using BookMK.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateAccountCommand:AsyncCommandBase
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
                });
            }
            catch (Exception ex)
            {

                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
