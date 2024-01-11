using BookMK.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookMK.Views
{
    /// <summary>
    /// Interaction logic for ResetPasswordForm.xaml
    /// </summary>
    public partial class ResetPasswordForm : Window
    {
        public ResetPasswordForm()
        {
            InitializeComponent();
            this.DataContext = new ResetPasswordViewModel();
        }
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        public bool IsValidEmail(string email)
        {
            // Regular expression for a basic email format check
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

            // Use Regex.IsMatch to check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if( String.IsNullOrEmpty(this.EmailField.Text) || !IsValidEmail(this.EmailField.Text ))
            {
                MessageBox.Show("Please check your input before continue!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                ResetPasswordViewModel vm = this.DataContext as ResetPasswordViewModel;
                vm.SendRecoveryMail();
                this.EmailField.IsReadOnly = true;
            }
        }
    }
}
