using BookMK.Commands;
using BookMK.Models;
using BookMK.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace BookMK.ViewModels
{
    public class ResetPasswordViewModel: ViewModelBase
    {
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {

                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public bool IsValidEmail(string email)
        {
            // Regular expression for a basic email format check
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

            // Use Regex.IsMatch to check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }
        public string Current6Digits { get; set; }

        public async void SendRecoveryMail()
        {
            await Task.Run(() =>
            {
                string str = MailService.Generate6Digits();
                Current6Digits = str;                
                if (Staff.IsStaffEmailExist(Email)!=null) 
                {
                    MailService.SendEmail(Email, "[NoReply] Password Reset", str);
                    MessageBox.Show("We've sent you a new password. Check your email.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetPassword = new ResetPasswordCommand(this,str);
                    ResetPassword.Execute(this);
                }
                else
                {
                    MessageBox.Show("We cant find any account with the entered email!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            });
        }
        ICommand ResetPassword{ get; set; }
        public ResetPasswordViewModel()
        {

        }
    }
}
