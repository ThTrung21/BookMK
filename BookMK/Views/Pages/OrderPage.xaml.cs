using BookMK.Models;
using BookMK.ViewModels;
using BookMK.ViewModels.ViewForm;
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
    /// Interaction logic for OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        Staff s { get; set; }
        public OrderPage()
        {
            InitializeComponent();
            DashBoardWindow main = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive) as DashBoardWindow;
            DashBoardViewModel vm = main.DataContext as DashBoardViewModel;
            Staff loggedinS = vm.CurrentStaff;
            s = loggedinS;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = await OrderViewModel.Initialize();
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertOrderForm f = new InsertOrderForm(s);
            f.ShowDialog();
            DataProvider<Order> db = new DataProvider<Order>(Order.Collection);
            List<Order> results = await db.ReadAllAsync();
            (this.DataContext as OrderViewModel).UpdateOrderList(results);
        }
        private DateTime _lastClickTime;
        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now - _lastClickTime).TotalMilliseconds < 500)
            {
                Grid g = sender as Grid;
                Order s = g.DataContext as Order;
                
                   
                ViewOrderForm f = new ViewOrderForm(s);
                f.ShowDialog();

               

            }
            _lastClickTime = DateTime.Now;
        }
    }
}
