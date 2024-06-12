using BookMK.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.ViewModels.ViewForm
{
    public class ViewImportViewModel: ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ViewImportViewModel));
        private Import _currentimport = new Import();
        public Import CurrentImport
        {
            get => _currentimport;
            set { _currentimport = value; OnPropertyChanged(nameof(CurrentImport)); }
        }

        private ObservableCollection<ImportItem> _importitemlist;
        public ObservableCollection<ImportItem> ImportItemList
        {
            get { return _importitemlist; }
            set { _importitemlist = value; OnPropertyChanged(nameof(ImportItemList)); }
        }

        private Int32 _id;
        public Int32 ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }

        private string _date;
        private string Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }
         ViewImportViewModel() { _logger.Information("ViewImportViewModel constructor called."); }
        public ViewImportViewModel(Import i) 
        {
            _logger.Information("ViewImportViewModel constructor with Import {a} parameter called.", i.ID);
            CurrentImport = i;
            
        }
    }
}
