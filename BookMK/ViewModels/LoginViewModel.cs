using Amazon.Runtime.Internal.Util;
using BookMK.Commands;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ILogger = Serilog.ILogger;

namespace BookMK.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(LoginViewModel));
        private string _username;
        public string Username
        {
            get { return _username; }
            set 
            { 

                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand LoginCommand { get; set; }
        
        public LoginViewModel()
        {
            _logger.Information("LoginViewModel initialized");


            LoginCommand = new LoginCommand(this);           
        }
    }
}
