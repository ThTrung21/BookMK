using BookMK.Models;
using BookMK.ViewModels;
using BookMK.Views.InsertForm;
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

            //do we really need to control which role get into customer?
            //
            //DashBoardWindow main = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive) as DashBoardWindow;
            //DashBoardViewModel vm = main.DataContext as DashBoardViewModel;
            //Staff loggedinS = vm.CurrentStaff;
            //s = loggedinS;
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = await CustomerViewModel.Initialize();
           
        }
        private DateTime _lastClickTime;

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertCustomerForm f = new InsertCustomerForm();
            f.ShowDialog();
            DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
            List<Customer> results = await db.ReadAllAsync();
            (this.DataContext as CustomerViewModel).UpdateCustomerList(results);
        }
    }
}
