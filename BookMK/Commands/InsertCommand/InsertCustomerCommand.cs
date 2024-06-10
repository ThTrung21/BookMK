using BookMK.Models;
using BookMK.ViewModels.InsertFormViewModels;
using Serilog;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.InsertCommand
{
    public class InsertCustomerCommand : AsyncCommandBase
    {
        private readonly InsertCustomerViewModel vm;

        public InsertCustomerCommand(InsertCustomerViewModel vm)
        {
            this.vm = vm;
        }

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
                String _Phone = vm.Phone;
                String _FullName = vm.FullName;
                String _Email = vm.Email;
                String _Address = vm.Address;

                if (string.IsNullOrWhiteSpace(_FullName) || string.IsNullOrWhiteSpace(_Email) || string.IsNullOrWhiteSpace(_Phone) || string.IsNullOrWhiteSpace(_Address))
                {
                    MessageBox.Show("Please fill in every field!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Customer.IsExisted(_FullName, _Phone) || Customer.IsExisted(_Phone))
                {
                    MessageBox.Show("This customer already exists, please check NAME and PHONE NUMBERS!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!IsValidEmail(_Email))
                {
                    MessageBox.Show("EMAIL format is incorrect!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Customer c = new Customer()
                {
                    ID = (int)_ID,
                    Phone = _Phone,
                    FullName = _FullName,
                    Email = _Email,
                    Address = _Address,
                };

                DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
                await db.InsertOneAsync(c);

                MessageBox.Show("Added a new customer!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                Window f = parameter as Window;
                f?.Close();

                // Log success
                Log.Information("New customer added: {CustomerName}, Email: {CustomerEmail}, Phone: {CustomerPhone}", _FullName, _Email, _Phone);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while inserting a new customer.");
            }
        }
    }
}
