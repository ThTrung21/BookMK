using BookMK.ViewModels.InsertFormViewModels;
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

namespace BookMK.Views.InsertForm
{
    /// <summary>
    /// Interaction logic for InsertCustomerForm.xaml
    /// </summary>
    public partial class InsertCustomerForm : Window
    {
        public InsertCustomerForm()
        {
            InitializeComponent();
            this.DataContext = new InsertCustomerViewModel();
        }
        private void PackIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }
        private void PackIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }
        private void CloseBtn_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        //phone textbox
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if (this.s == null)
            this.DataContext = await InsertCustomerViewModel.Initialize();
            //else
            //    this.DataContext = new InsertStaffViewModel(s);
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertCustomerViewModel vm = this.DataContext as InsertCustomerViewModel;
            //if(vm.FullName==null|| vm.Phone==null||)
            if(vm.InsertCustomer != null)
            {
                vm.InsertCustomer.Execute(this);
            }
        }
    }
}
