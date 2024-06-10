using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using BookMK.Models;
using BookMK.ViewModels.InsertFormViewModels;
using Serilog;

namespace BookMK.Commands.InsertCommand
{
    public class InsertStaffCommand : AsyncCommandBase
    {
        private readonly InsertStaffViewModel vm;

        public InsertStaffCommand(InsertStaffViewModel vm)
        {
            this.vm = vm;
        }

        // Check email format
        static bool IsValidEmail(string email)
        {
            // Regular expression for a basic email format check
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

            // Use Regex.IsMatch to check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                Int64 _ID = vm.ID;
                String _Username = vm.Phone;
                String _PasswordHash = Staff.HashPassword("12345");
                String _Phone = vm.Phone;
                String _FullName = vm.FullName;
                String _Email = vm.Email;
                bool _IsVerified = false;
                string _Role = "staff";

                if ((_FullName == null) || (_Email == null) || (_Phone == null))
                {
                    MessageBox.Show("Please fill in every fields!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Staff.IsExisted(_FullName, _Phone) || Staff.IsExisted(_Phone))
                {
                    MessageBox.Show("This Staff already exists, please check NAME and PHONE NUMBERS!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!IsValidEmail(_Email))
                {
                    MessageBox.Show("EMAIL format is incorrect!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Staff c = new Staff()
                {
                    ID = (int)_ID,
                    Username = _Username,
                    PasswordHash = _PasswordHash,
                    Phone = _Phone,
                    FullName = _FullName,
                    Email = _Email,
                    IsVerified = _IsVerified,
                    Role = _Role
                };

                DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                await db.InsertOneAsync(c);

                MessageBox.Show("Added a new staff!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                Window f = parameter as Window;
                f?.Close();

                // Log success
                Log.Information("New staff added: ID - {StaffID}, FullName - {FullName}, Email - {Email}", c.ID, c.FullName, c.Email);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while inserting a new staff.");
            }
        }
    }
}
