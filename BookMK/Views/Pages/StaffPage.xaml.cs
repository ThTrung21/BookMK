using BookMK.Models;
using BookMK.ViewModels;
using BookMK.Views.InsertForm;
using BookMK.Views.ViewForm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for StaffPage.xaml
    /// </summary>
    public partial class StaffPage : Page
    {
        public StaffPage()
        {
            InitializeComponent();
           
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = await StaffViewModel.Initialize();
           
        }
        
        

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertStaffForm f = new InsertStaffForm();
            f.ShowDialog();
            DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
            List<Staff> results = await db.ReadAllAsync();
            (this.DataContext as StaffViewModel).UpdateListStaff(results);
        }


        private DateTime _lastClickTime;
        private async void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now - _lastClickTime).TotalMilliseconds < 500)
            {
                Grid g = sender as Grid;
                Staff s = g.DataContext as Staff;

                ViewStaffForm f = new ViewStaffForm(s);
                f.ShowDialog();

                //update the list
                if (this.DataContext != null)
                {
                    StaffViewModel vm = this.DataContext as StaffViewModel;
                    DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
                    vm.UpdateListStaff(await db.ReadAllAsync());
                }

            }
            _lastClickTime = DateTime.Now;
        }
    }
}
