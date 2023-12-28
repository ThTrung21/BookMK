using BookMK.Models;
using BookMK.ViewModels;
using BookMK.Views.InsertForm;
using BookMK.Views.ViewForm;
using BookMK.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookMK.Views.Pages
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage  : System.Windows.Controls.Page
    {
        //Customer s;
        public CustomerPage()
        {
            InitializeComponent();
            ComboboxFilter.ItemsSource = new List<string>() {"Name", "Phone" };

            
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = await CustomerViewModel.Initialize();
           
        }
       

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertCustomerForm f = new InsertCustomerForm();
            f.ShowDialog();
            DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
            List<Customer> results = await db.ReadAllAsync();
            (this.DataContext as CustomerViewModel).UpdateCustomerList(results);
        }
        private DateTime _lastClickTime;
        private async void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now - _lastClickTime).TotalMilliseconds < 500)
            {
                Grid g = sender as Grid;
                Customer c = g.DataContext as Customer;

                ViewCustomerForm f = new ViewCustomerForm(c);
                f.ShowDialog();

                //update the list
                if (this.DataContext != null)
                {
                    CustomerViewModel vm = this.DataContext as CustomerViewModel;
                    DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
                    vm.UpdateCustomerList(await db.ReadAllAsync());
                }

            }
            _lastClickTime = DateTime.Now;
        }
    }
}
