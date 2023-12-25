using BookMK.ViewModels;
using BookMK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    public class UpdateStaffCommand : AsyncCommandBase
    {
        SettingViewModel vm;
        public UpdateStaffCommand(SettingViewModel vm) { this.vm = vm; }
        public override async Task ExecuteAsync(object parameter) 
        {
            MessageBoxResult asking = MessageBox.Show("Are you sure to save change?", "REMINDING",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
