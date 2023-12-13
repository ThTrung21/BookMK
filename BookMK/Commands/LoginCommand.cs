using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookMK.Models;
//using BookMK.Windows;
using BookMK.ViewModels;
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
        public override void Execute(object parameter)
        {
            string password = loginViewModel.password;
            //DataProvider<Account> db = new DataProvider<Account>(Account.Collection) ;
            //string password = loginViewModel.password;
            //extract password from account table from database and compare to user entered password
            MessageBox.Show("Heelo world");
        }
    }
}
