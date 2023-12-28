using BookMK.Models;
using BookMK.ViewModels.ViewForm;
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

namespace BookMK.Views.ViewForm
{
    /// <summary>
    /// Interaction logic for ViewCustomerForm.xaml
    /// </summary>
    public partial class ViewCustomerForm : Window
    {
        Customer c { get; set; }

        public ViewCustomerForm()
        {
            InitializeComponent();
        }
        public ViewCustomerForm(Customer c)
        {
            InitializeComponent();
            this.DataContext = new ViewCustomerViewModel(c);
        }
        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true; // Suppress non-numeric input
            }
        }
      
        
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewCustomerViewModel vm = this.DataContext as ViewCustomerViewModel;
            if(vm.UpdateCustomer!=null)
            {
                vm.UpdateCustomer.Execute(this);
            }
        }
        private void CloseBtn_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
