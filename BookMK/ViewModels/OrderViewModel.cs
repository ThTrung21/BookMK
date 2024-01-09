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
using System.Windows.Controls;

namespace BookMK.ViewModels
{
    public class OrderViewModel:ViewModelBase
    {
        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set { _orders = value; OnPropertyChanged(nameof(Orders)); }
        }


        public ObservableCollection<string> ComboBoxItems { get; set; } = new ObservableCollection<string>(new List<string>() { "Customer", "Phone","Staff" });
        private int _selectedIndex = 0;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value;/* Search()*/; OnPropertyChanged(nameof(SelectedIndex)); }
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
        private Customer _buyer;
        public Customer Buyer
        {
            get
            {
                return _buyer;
            }
            set
            {
                _buyer = value;
                OnPropertyChanged(nameof(Buyer));
            }
        }
        private Staff _cashier;
        public Staff Cashier
        {
            get
            {
                return _cashier;
            }
            set
            {
                _cashier = value;
                OnPropertyChanged(nameof(Cashier));
            }
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

        private double _value;
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }









        public static async Task<OrderViewModel> Initialize()
        {
            OrderViewModel viewModel = new OrderViewModel();
            await viewModel.InitializeAsync();
            return viewModel;
        }
        private async Task InitializeAsync()
        {
            DataProvider<Order> db = new DataProvider<Order>(Order.Collection);
            List<Order> allorder = await db.ReadAllAsync();
            this._orders = new ObservableCollection<Order>(allorder);

        }
        public void UpdateOrderList(List<Order> i)
        {
            this.Orders.Clear();
            foreach (Order c in i)
            {
                Orders.Add(c);
            }
        }


        private async void Search()
        {
            await Task.Run(async () =>
            {
                DataProvider<Order> db = new DataProvider<Order>(Order.Collection);
                string searchInput = SearchString.Trim();
                List<Order> results = new List<Order>();
                switch (_selectedIndex)
                {
                    //buyer name
                    case 0:
                        {
                            FilterDefinition<Order> filter = Builders<Order>.Filter.Regex("CustomerName", new BsonRegularExpression(searchInput, "i"));
                            results = db.ReadFiltered(filter);
                        }
                        break;

                    //buyer phone
                    case 1:
                        {
                            FilterDefinition<Order> filter = Builders<Order>.Filter.Regex("CustomerPhone", new BsonRegularExpression(searchInput, "i"));
                            results = db.ReadFiltered(filter);
                        }
                        break;
                    //case 2:
                    //    {
                    //        FilterDefinition<Order> filter = Builders<Order>.Filter.Regex("Time", new BsonRegularExpression(searchInput, "i"));
                    //        results = db.ReadFiltered(filter);
                    //    }
                    //    break;
                    case 2:
                        {
                            FilterDefinition<Order> filter = Builders<Order>.Filter.Regex("StaffName", new BsonRegularExpression(searchInput, "i"));
                            results = db.ReadFiltered(filter);
                        }
                        break;
                    default:
                        return;
                }
                Application.Current.Dispatcher.Invoke(() => {
                    Orders.Clear();
                    foreach (Order c in results)
                    {
                        Orders.Add(c);
                    }
                });
            });
        }


    }
}
