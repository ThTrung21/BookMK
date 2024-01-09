using BookMK.Models;
using BookMK.ViewModels;
using BookMK.ViewModels.InsertFormViewModels;
using BookMK.Windows;
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
using System.Windows.Shapes;

namespace BookMK.Views.InsertForm
{
    /// <summary>
    /// Interaction logic for InsertOrderForm.xaml
    /// </summary>
    public partial class InsertOrderForm : Window
    {
        Staff s { get; set; }
        public InsertOrderForm()
        {
            InitializeComponent();
        }
        public InsertOrderForm(Staff a)
        {
            InitializeComponent();


            s = a;
            this.Checkout.Visibility = Visibility.Collapsed;
            this.backbtn.Visibility = Visibility.Collapsed;
            this.CheckoutBtn.Visibility = Visibility.Collapsed;
            this.DataContext = new InsertOrderViewModel(s);
        }
       
        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }
        

        private void PackIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true; // Suppress non-numeric input
            }
        }

        private DateTime _lastClickTime;
        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now - _lastClickTime).TotalMilliseconds < 500)
            {
                //var selectedItem = ((ListView)sender).SelectedItem as ImportItem;
                Grid g = sender as Grid;
                OrderItem i = g.DataContext as OrderItem;
                // Check if an item is selected
                if (i != null)
                {
                    if (i.isGifted == true)
                        return;
                    // Execute the remove command in the ViewModel
                    ((InsertOrderViewModel)DataContext).RemoveItemCommand.Execute(i);
                }
            }
            _lastClickTime = DateTime.Now;
        }

        private void AddItemBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void  Window_Loaded_1(object sender, RoutedEventArgs e)
        {
           // this.DataContext = await InsertOrderViewModel.Initialize();
            
        }

        private void Proceedbtn_Click(object sender, RoutedEventArgs e)
        {
            InsertOrderViewModel vm= this.DataContext as InsertOrderViewModel;
            if(vm.OrderItemList==null)
            {
                MessageBox.Show("Error! There's nothing in your order", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            this.AddBook.Visibility= Visibility.Collapsed;
            this.Proceedbtn.Visibility = Visibility.Collapsed;
            this.Checkout.Visibility= Visibility.Visible;
            this.backbtn.Visibility = Visibility.Visible;
            this.CheckoutBtn.Visibility = Visibility.Visible;
        }

        private void backbtn_Click(object sender, RoutedEventArgs e)
        {
            InsertOrderViewModel vm = this.DataContext as InsertOrderViewModel;
            vm.SelectedDiscount = null;
            this.AddBook.Visibility = Visibility.Visible;
            this.Proceedbtn.Visibility = Visibility.Visible;
            this.Checkout.Visibility = Visibility.Collapsed;
            this.backbtn.Visibility = Visibility.Collapsed;
            this.CheckoutBtn.Visibility = Visibility.Collapsed;
        }

        private void CheckoutBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertOrderViewModel vm = this.DataContext as InsertOrderViewModel;
            if(vm.InsertOrder!=null)
            {
                vm.InsertOrder .Execute(this);
            }
        }
    }
}
