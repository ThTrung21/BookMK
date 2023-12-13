using BookMK.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookMK.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        private string _password;
        public string password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(password));
            }
        }
        public ICommand LoginCommand { get; set; }
        
        public LoginViewModel()
        {
            LoginCommand = new LoginCommand(this);
            
        }
    }
}
