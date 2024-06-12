using BookMK.Models;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.ViewModels
{
    public class StaffViewModel: ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(StaffViewModel));
        private ObservableCollection<Staff> _staffs;
        public ObservableCollection<Staff> Staffs
        {
            get { return _staffs; }
            set { _staffs = value; OnPropertyChanged(nameof(Staffs)); }
        }

        //search for a staff
        private string _searchString;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                Search();
                OnPropertyChanged(nameof(SearchString));
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
        private string _fullname;
        public string FullName
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        private string _phone;
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        private bool _isverified;
        public bool IsVerified
        {
            get
            {
                return _isverified;
            }
            set
            {
                _isverified= value;
                OnPropertyChanged(nameof(IsVerified));
            }
        }





        public static async Task<StaffViewModel> Initialize()
        {
            StaffViewModel viewmodel = new StaffViewModel();
            await viewmodel.InitializeAsync(); _logger.Information("StaffViewModel initialized");
            return viewmodel;
        }
        private async Task InitializeAsync()
        {
            DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
            List<Staff> stf = await db.ReadAllAsync();
            int xx = stf.Count;
            this.Staffs = new ObservableCollection<Staff>(stf);
            
        }


        public void UpdateListStaff(List<Staff> results)
        {
            this.Staffs.Clear();
            foreach (Staff s in results)
            {
                Staffs.Add(s);
            }
            _logger.Information("Showing updated import list");
        }



        private async void Search()
        {
            await Task.Run(async () =>
            {
                DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                string searchInput = SearchString.Trim().ToLower();
                List<Staff> results = new List<Staff>();

                FilterDefinition<Staff> filter = Builders<Staff>.Filter.Where(
                    s => (s.FullName.ToLower().Trim().Contains(searchInput) ) );
                //still nedd to implement a way to not show admin

                //Wait for future staff role to be added
               //&& s.Role.Equals("staff"))


                results = db.ReadFiltered(filter);
                           

                
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Staffs.Clear();
                    foreach (Staff s in results)
                    {
                        if (s.Role != "admin")
                            Staffs.Add(s);
                    }
                });
            });
        }
    }
}
