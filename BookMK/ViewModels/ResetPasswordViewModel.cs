using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookMK.ViewModels
{
    public class ResetPasswordViewModel: ViewModelBase
    {
        private string _email;
        public string email
        {
            get { return _email; }
            set
            {

                _email = value;
                OnPropertyChanged(nameof(email));
            }
        }

        ICommand ResetPasswordCommand{ get; set; }
        public ResetPasswordViewModel()
        {

        }
    }
}
