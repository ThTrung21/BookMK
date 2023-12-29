using BookMK.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.ViewModels.ViewForm
{
    public class ImportViewModel: ViewModelBase
    {
        private ObservableCollection<Import> _imports;
        public ObservableCollection<Import> Imports
        {
            get { return _imports; }
            set { _imports = value; OnPropertyChanged(nameof(Imports)); }
        }
        private int _id;
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }
        private DateTime _date;
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }





    }
}
