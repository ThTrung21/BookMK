using BookMK.Models;
using BookMK.ViewModels;
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
using System.Windows.Shapes;

namespace BookMK.Windows
{
    /// <summary>
    /// Interaction logic for DashBoardWindow.xaml
    /// </summary>
    public partial class DashBoardWindow : Window
    {
        public DashBoardWindow()
        {
            InitializeComponent();
            this.DataContext = new DashBoardViewModel();
            (this.DataContext as DashBoardViewModel).SwitchBookPage();
        }
        public Staff s { get; set; }

        public DashBoardWindow(Staff loggedinStaff)
        {
            InitializeComponent();
            this.DataContext = new DashBoardViewModel(loggedinStaff);
            (this.DataContext as DashBoardViewModel).SwitchBookPage();
            this.s = loggedinStaff;
            if (s.Role != "admin")
            {
                //this.AuthorBtn.Visibility = Visibility.Collapsed;
                this.StaffBtn.Visibility = Visibility.Collapsed;
                this.Importbtn.Visibility = Visibility.Collapsed;
                this.DiscountBtn.Visibility = Visibility.Collapsed;
                this.HomeBtn.Visibility = Visibility.Collapsed;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RadioButton_Checked1(object sender, RoutedEventArgs e)
        {
            (this.DataContext as DashBoardViewModel).SwitchBookPage();
        }
        private void  AuthorBtn_Checked(object sender, RoutedEventArgs e)
        {
            (this.DataContext as DashBoardViewModel).SwitchAuthorPage();
        }

        private void CustomerBtn_Checked(object sender, RoutedEventArgs e)
        {
            (this.DataContext as DashBoardViewModel).SwitchCustomerPage();
        }

        private void StaffBtn_Checked(object sender, RoutedEventArgs e)
        {
            (this.DataContext as DashBoardViewModel).SwitchStaffPage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (this.DataContext as DashBoardViewModel).SwitchSettingPage();
        }

        private void Importbtn_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as DashBoardViewModel).SwitchImportPage();
        }

        private void DiscountBtn_Checked(object sender, RoutedEventArgs e)
        {
            (this.DataContext as DashBoardViewModel).SwitchDiscountPage();
        }

        private void OrderBtn_Checked(object sender, RoutedEventArgs e)
        {
            (this.DataContext as DashBoardViewModel).SwitchOrderPage();
        }

        private void PackIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult asking= MessageBox.Show("Are you sure want to quit?","Info",MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (asking == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else return;
        }
    }
}
