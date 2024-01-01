
using BookMK.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookMK.ViewModels
{
    public class ImportViewModel : ViewModelBase
    {
        private ObservableCollection<Import> _imports;
        public ObservableCollection<Import> Imports
        {
            get { return _imports; }
            set { _imports = value; OnPropertyChanged(nameof(Imports)); }
        }
        private DateTime _selectedstartdate;
        public DateTime SelectedStartDate
        {
            get { return _selectedstartdate; }
            set
            {
                _selectedstartdate = value;

                OnPropertyChanged(nameof(SelectedStartDate));
            }
        }
        private DateTime _selectedenddate;
        public DateTime SelectedEndDate
        {
            get { return _selectedenddate; }
            set
            {
                _selectedenddate = value;

                OnPropertyChanged(nameof(SelectedEndDate));
            }
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
        private double _totalprice;
        public double TotalPrice
        {
            get
            {
                return _totalprice;
            }
            set
            {
                _totalprice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }


        public ICommand SearchImport{get;set;}




        public ImportViewModel()
        {
            SelectedStartDate = DateTime.Now;
            SelectedEndDate = DateTime.Now;
           
        }

        public static async Task<ImportViewModel> Initialize()
        {
            ImportViewModel viewModel = new ImportViewModel();
            await viewModel.InitializeAsync();
            return viewModel;
        }
        private async Task InitializeAsync()
        {
            DataProvider<Import> db = new DataProvider<Import>(Import.Collection);
            List<Import> AllImports = await db.ReadAllAsync();
            this._imports = new ObservableCollection<Import>(AllImports);
            
        }
        public void UpdateImportList(List<Import> i)
        {
            this.Imports.Clear();
            foreach (Import c in i)
            {
                Imports.Add(c);
            }
        }


        public async void Search()
        {
            await Task.Run(async () =>
            {
                DataProvider<Import> db = new DataProvider<Import>(Import.Collection);
                //string searchInput = SearchString.Trim().ToLower();

                DateTime StartDate = SelectedStartDate;
                DateTime EndDate = SelectedEndDate;

                if(StartDate>EndDate)
                {
                    MessageBox.Show("End date cannot be before Start date!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if(SelectedStartDate==null || SelectedEndDate==null)
                {
                    MessageBox.Show("No date detected!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                List<Import> results = new List<Import>();

                var builder = Builders<Import>.Filter;
                var filter = builder.Gte("Time", StartDate) & builder.Lte("Time", EndDate);

                   
                

                results = db.ReadFiltered(filter);



                Application.Current.Dispatcher.Invoke(() =>
                {
                    Imports.Clear();
                    foreach (Import s in results)
                    {
                        
                            Imports.Add(s);
                    }
                });
            });
        }
    }
}
