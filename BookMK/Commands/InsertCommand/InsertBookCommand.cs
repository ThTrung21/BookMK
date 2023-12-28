using BookMK.Models;
using BookMK.ViewModels.InsertFormViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.InsertCommand
{
    public class InsertBookCommand : AsyncCommandBase
    {
        private readonly InsertBookViewModel vm;
        public InsertBookCommand(InsertBookViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                //Int64 _ID = vm.ID;
                //String _Username = vm.Phone;
                //String _PasswordHash = Staff.HashPassword("12345");
                //String _Title = vm.Title;

                //bool _IsVerified = false;
                //string _Role = "staff";
                //if ((_FullName == null) || (_Email == null) || (_Phone == null))
                //{
                //    MessageBox.Show("Please fill in every fields!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}
                //if (Staff.IsExisted(_FullName, _Phone) || Staff.IsExisted(_Phone))
                //{
                //    MessageBox.Show("This Staff already exists, please check NAME and PHONE NUMBERS!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}

                //if (!IsValidEmail(_Email))
                //{
                //    MessageBox.Show("EMAIL format is incorrect!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}

                //Staff c = new Staff()
                //{
                //    ID = (int)_ID,
                //    Username = _Username,
                //    PasswordHash = _PasswordHash,
                //    Phone = _Phone,
                //    FullName = _FullName,
                //    Email = _Email,
                //    IsVerified = _IsVerified,
                //    Role = _Role
                //};

                //DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                //await db.InsertOneAsync(c);

                //MessageBox.Show("Added a new staff!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                //Window f = parameter as Window;
                //f?.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
