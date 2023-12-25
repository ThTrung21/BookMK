using BookMK.Commands;
using BookMK.Commands.InsertCommand;
using BookMK.Models;
using BookMK.ViewModels.InsertFormViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookMK.ViewModels
{
    public class SettingViewModel: ViewModelBase
    {
        private Staff _currentStaff;
        public Staff CurrentStaff
        {
            get => _currentStaff;
            set { _currentStaff = value; OnPropertyChanged(nameof(CurrentStaff)); }
        }
        private Customer _currentCustomer;
        public Customer CurrentCustomer
        {
            get => _currentCustomer;
            set { _currentCustomer = value; OnPropertyChanged(nameof(CurrentCustomer)); }
        }

        private string _newpassword;
        public string NewPassword
        {
            get => _newpassword;
            set { _newpassword = value; OnPropertyChanged(nameof(NewPassword)); }
        }

        public ICommand ChangePassword { get; set; }

        

        public SettingViewModel() { }
        public SettingViewModel(Staff s)
        {
           
            this.CurrentStaff = s;
            
            this.ChangePassword = new ChangePasswordCommand(this,1);
        
        }
        public SettingViewModel(Customer c)
        {

            this.CurrentCustomer = c;

            this.ChangePassword = new ChangePasswordCommand(this, 2);

        }


        //public static async Task<SettingViewModel> Initialize(int Kind)
        //{
        //    SettingViewModel viewModel = new SettingViewModel(Kind);
        //    await viewModel.IntializeAsync();
        //    return viewModel;
        //}

        //private async Task IntializeAsync()
        //{
        //    await Task.Run(async () =>
        //    {
        //        // Simulate an asynchronous operation
        //        await Task.Delay(1000);


        //        ChangePassword = new ChangePasswordCommand(this,_kind);
        //    });
        //}
    }
}
