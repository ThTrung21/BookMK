using BookMK.Commands;
using BookMK.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookMK.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
       

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
        public MainViewModel()
        {
            CurrentViewModel = new DashBoardViewModel(this);
        }
        
    }
}
