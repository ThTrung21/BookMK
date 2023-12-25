using BookMK.Models;
using BookMK.ViewModels;
using MongoDB.Driver;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands
{
    public class ChangePasswordCommand : AsyncCommandBase
    {
        private readonly SettingViewModel vm;
        int kind;
        public ChangePasswordCommand(SettingViewModel vm, int kind)
        {
            this.vm = vm;
            this.kind = kind;
        }
        public override async Task ExecuteAsync(object parameter)
        {

            MessageBoxResult asking = MessageBox.Show("Are you sure to save change?", "REMINDING",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (asking == MessageBoxResult.No) return;

            try
            {
                String _NewPassword = Staff.HashPassword(vm.NewPassword);
                if (_NewPassword == null)
                {
                    MessageBox.Show("Your new password is invalid!!!");
                    return;
                }
                //for staff
                if(kind==1)
                {

                    Staff s = vm.CurrentStaff;
                    await Task.Run(() =>
                    {
                        DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                        FilterDefinition<Staff> filter = Builders<Staff>.Filter.Eq("_id", s.ID);
                        UpdateDefinition<Staff> update = Builders<Staff>.Update
                       
                        .Set(x => x.PasswordHash,_NewPassword);
                        db.Update(filter, update);

                        MessageBox.Show("Your password has been updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window f = parameter as Window;
                        f?.Close();

                    });
                }







            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
