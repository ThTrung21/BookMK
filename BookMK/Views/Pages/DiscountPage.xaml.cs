using BookMK.Models;
using BookMK.ViewModels;

using BookMK.ViewModels.ViewForm;
using BookMK.Views.InsertForm;
using BookMK.Views.ViewForm;
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
    /// Interaction logic for DiscountPage.xaml
    /// </summary>
    public partial class DiscountPage : Page
    {
        public DiscountPage()
        {
            InitializeComponent();
            this.DataContext = new DiscountViewModel();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = await DiscountViewModel.Initialize();
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertDiscountForm f = new InsertDiscountForm();
            f.ShowDialog();
            DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
            List<Discount> results = await db.ReadAllAsync();
            (this.DataContext as DiscountViewModel).UpdateDiscountList(results);
        }
        private DateTime _lastClickTime;
        private async void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now - _lastClickTime).TotalMilliseconds < 500)
            {
                Grid g = sender as Grid;
                Discount s = g.DataContext as Discount;

                ViewDiscountForm f = new ViewDiscountForm(s);
                f.ShowDialog();

                //update the list
                if (this.DataContext != null)
                {
                    DiscountViewModel vm = this.DataContext as DiscountViewModel;
                    DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
                    vm.UpdateDiscountList(await db.ReadAllAsync());
                }

            }
            _lastClickTime = DateTime.Now;
        }
    }
}
