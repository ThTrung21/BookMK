using BookMK.ViewModels;
using BookMK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateStaffCommand : AsyncCommandBase
    {
        ViewStaffViewModel vm;
        public UpdateStaffCommand(ViewStaffViewModel vm) { this.vm = vm; }
        public override async Task ExecuteAsync(object parameter)
        {
            Staff updatedStaff = vm.CurrentStaff;

            try
            {
                // Check if both Name and Note are empty


                await Task.Run(() =>
                {
                    FilterDefinition<Staff> filter = Builders<Staff>.Filter.Eq(x => x.ID, updatedStaff.ID);
                    UpdateDefinition<Staff> update = Builders<Staff>.Update
                        .Set(x => x.FullName, updatedStaff.FullName)
                        .Set(x => x.Username, updatedStaff.Phone)
                        .Set(x => x.Phone, updatedStaff.Phone);
                        
                    //more attributes

                    DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                    db.Update(filter, update);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Staff updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
