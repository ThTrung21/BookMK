using BookMK.Models;
using BookMK.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands
{
    public class ResetPasswordCommand:AsyncCommandBase
    {
        private readonly ResetPasswordViewModel viewModel;
        private string reset;
        public ResetPasswordCommand(ResetPasswordViewModel viewModel,string newpass)
        {
            this.viewModel = viewModel;
            reset = newpass;
        }
        public override async Task ExecuteAsync(object parameter)
        {
           
           

            try
            {


                //getstaffbyemail
                String _NewPassword = Staff.HashPassword(reset);

             
                await Task.Run(() =>
                {
                    DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                    FilterDefinition<Staff> filter = Builders<Staff>.Filter.Eq(x=>x.Email,viewModel.Email);
                    UpdateDefinition<Staff> update = Builders<Staff>.Update

                    .Set(x => x.PasswordHash, _NewPassword);
                    db.Update(filter, update);

                    
                    Window f = parameter as Window;
                    f?.Close();

                });








            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
