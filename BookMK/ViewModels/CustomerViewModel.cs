using BookMK.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.ViewModels
{
    public class CustomerViewModel: ViewModelBase
    {
        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set { _customers = value; OnPropertyChanged(nameof(Customers)); }
        }
        private string _searchString = "";
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
        private int _purchasepoint;
        public int PurchasePoint
        {
            get
            {
                return _purchasepoint;
            }
            set
            {
                _purchasepoint = value;
                OnPropertyChanged(nameof(PurchasePoint));
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
                _isverified = value;
                OnPropertyChanged(nameof(IsVerified));
            }
        }

        //filter
        public ObservableCollection<string> ComboBoxItems { get; set; } = new ObservableCollection<string>(new List<string>() { "Name","Phone number" });
        private int _selectedIndex = 0;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; Search(); OnPropertyChanged(nameof(SelectedIndex)); }
        }
      
        public CustomerViewModel()
        {

        }

        // update list---------------
        public void UpdateCustomerList(List<Customer> customers)
        {
            this.Customers.Clear();
            foreach (Customer c in customers)
            {
                Customers.Add(c);
            }
        }
        public static async Task<CustomerViewModel> Initialize()
        {
            CustomerViewModel viewModel = new CustomerViewModel();
            await viewModel.InitializeAsync();
            return viewModel;
        }
        private async Task InitializeAsync()
        {
            DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
            List<Customer> AllCustomers = await db.ReadAllAsync();
            this._customers = new ObservableCollection<Customer>(AllCustomers);
            
        }
        // Search--------------------------
        private async void Search()
        {
            await Task.Run(async () =>
            {
                DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
                string searchInput = SearchString.Trim();
                List<Customer> results = new List<Customer>();
                switch (_selectedIndex)
                {
                    
                    case 0:
                        {
                            FilterDefinition<Customer> filter = Builders<Customer>.Filter.Regex("FullName", new BsonRegularExpression(searchInput, "i"));
                            results = db.ReadFiltered(filter);
                        }
                        break;
                    case 1:
                        {
                            FilterDefinition<Customer> filter = Builders<Customer>.Filter.Regex("Phone", new BsonRegularExpression(searchInput, "i"));
                            results = db.ReadFiltered(filter);
                        }
                        break;
                    default:
                        return;
                }
                Application.Current.Dispatcher.Invoke(() => {
                    Customers.Clear();
                    foreach (Customer c in results)
                    {
                        Customers.Add(c);
                    }
                });
            });
        }





    }
}
