using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookMK.Models;
//using BookMK.Windows;
using BookMK.ViewModels;
using BookMK.Windows;
using MongoDB.Driver;

namespace BookMK.Commands
{
    public class LoginCommand : CommandBase
    {
        LoginViewModel loginViewModel;
        public LoginCommand(LoginViewModel loginViewModel)
        {
            this.loginViewModel = loginViewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public override void Execute(object parameter)
        {
            string username = loginViewModel.Username;
            string password = loginViewModel.Password;

            if (username == null || password == null)
            {
                MessageBox.Show("Please enter your username and password!!!");
                return;
            }

            string Hpassword = HashPassword(password);
            
         
            DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);

            // extract password from staff table from database and compare to user entered password
            FilterDefinition<Staff> filterLogin = Builders<Staff>.Filter.Where(s => s.Username == username && s.PasswordHash == Hpassword);
            var matchingStaff = db.ReadFiltered(filterLogin);
            if (matchingStaff != null)
            {
                Staff loggedInStaff = null;
                if (matchingStaff.Count > 0)
                {
                    loggedInStaff = matchingStaff[0];
                    Window f = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                    f.Hide();
                    DashBoardWindow ff = new DashBoardWindow(loggedInStaff);
                    ff.ShowDialog();
                    f.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect pass!");
                }
            }
        }
    }
}






