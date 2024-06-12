using BookMK.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookMK.ViewModels.ViewForm
{
    public class DiscountViewModel: ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(DiscountViewModel));

        private ObservableCollection<Discount> _discounts;
        public ObservableCollection<Discount> Discounts
        {
            get { return _discounts; }
            set { _discounts = value; OnPropertyChanged(nameof(Discounts)); }
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
        
        private DateTime _time;
        public DateTime Time
        {
            get => _time;
            set { _time=value; OnPropertyChanged(nameof(Time));}
        }
        private string _type;
        public string Type
        {
            get=> _type;
            set { _type=value; OnPropertyChanged(nameof(Type)); }
        }
        private int _value;
        public int Value
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


        public ObservableCollection<string> ComboBoxItems { get; set; } = new ObservableCollection<string>(new List<string>() { "All", "Percentage Off","Amount off","BOGO promotion" });
        private int _selectedIndex = 0;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; Search(); OnPropertyChanged(nameof(SelectedIndex)); }
        }

        public void UpdateDiscountList(List<Discount> discounts)
        {
            this.Discounts.Clear();
            foreach (Discount d in discounts)
            {
                
                    Discounts.Add(d);
            }
        }

      

        public static async Task<DiscountViewModel> Initialize()
        {
            _logger.Information("Initializing DiscountViewModel.");
            DiscountViewModel viewModel = new DiscountViewModel();
            await viewModel.InitializeAsync();
            return viewModel;
        }
        private async Task InitializeAsync()
        {
            _logger.Information("Starting asynchronous initialization of DiscountViewModel.");

            try
            {
                DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
            List<Discount> a = await db.ReadAllAsync();
            this._discounts = new ObservableCollection<Discount>(a);
                _logger.Information("Asynchronous initialization of DiscountViewModel completed.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred during the asynchronous initialization of DiscountViewModel.");
            }

        }

        private async void Search()
        {
            await Task.Run(async () =>
            {
            try
            {
                DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
               
                List<Discount> results = new List<Discount>();
                switch (_selectedIndex)
                {
                    case 0:
                        {
                            //FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq(d => d.Type, "Percentage");
                            results = db.ReadAll();
                        }
                        break;
                    case 1:
                        {
                            FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq(d => d.Type, "Percentage");
                            results = db.ReadFiltered(filter);
                        }
                        break;
                    case 2:
                        {
                            FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq(d => d.Type, "Amount");
                            results = db.ReadFiltered(filter);
                        }
                        break;
                    case 3:
                        {
                            FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq(d => d.Type, "BOGO");
                            results = db.ReadFiltered(filter);
                        }
                        break;
                    default:
                        return;
                }
                Application.Current.Dispatcher.Invoke(() => {
                    Discounts.Clear();
                    foreach (Discount c in results)
                    {
                        Discounts.Add(c);
                    }
                });
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "An error occurred during the search.");
                }
            });
        }
    }
}
