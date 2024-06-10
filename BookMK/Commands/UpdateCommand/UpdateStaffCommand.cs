using BookMK.Models;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateStaffCommand : AsyncCommandBase
    {
        private readonly ViewStaffViewModel vm;

        public UpdateStaffCommand(ViewStaffViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Staff updatedStaff = vm.CurrentStaff;

            try
            {
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

                    // Log success
                    Log.Information("Staff updated successfully: ID - {StaffID}, FullName - {FullName}, Username - {Username}, Phone - {Phone}", updatedStaff.ID, updatedStaff.FullName, updatedStaff.Username, updatedStaff.Phone);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while updating staff: ID - {StaffID}, FullName - {FullName}, Username - {Username}, Phone - {Phone}", updatedStaff.ID, updatedStaff.FullName, updatedStaff.Username, updatedStaff.Phone);
            }
        }
    }
}
