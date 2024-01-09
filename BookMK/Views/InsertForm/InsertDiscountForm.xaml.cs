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
    /// Interaction logic for InsertDiscountForm.xaml
    /// </summary>
    public partial class InsertDiscountForm : Window
    {
        public InsertDiscountForm()
        {
            InitializeComponent();
            this.DataContext = new InsertDiscountViewModel();
        }
        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.DataContext = await InsertDiscountViewModel.Initialize();
        }

        private void PackIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void InsertBtn3_Click(object sender, RoutedEventArgs e)
        {
            InsertDiscountViewModel vm = this.DataContext as InsertDiscountViewModel;
            if (vm.InsertDiscount3 != null)
            {
                vm.InsertDiscount3.Execute(this);
            }
        }
       
        private void InsertBtn2_Click(object sender, RoutedEventArgs e)
        {
            InsertDiscountViewModel vm = this.DataContext as InsertDiscountViewModel;
            if (vm.InsertDiscount2 != null)
            {
                vm.InsertDiscount2.Execute(this);
            }
        }

        private void InsertBtn1_Click(object sender, RoutedEventArgs e)
        {
            InsertDiscountViewModel vm = this.DataContext as InsertDiscountViewModel;
            if (vm.InsertDiscount1 != null)
            {
                vm.InsertDiscount1.Execute(this);
            }

        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true; // Suppress non-numeric input
            }
        }
        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true; // Suppress non-numeric input
            }
        }
        private void TextBox_PreviewTextInput_2(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true; // Suppress non-numeric input
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            InsertDiscountViewModel vm = this.DataContext as InsertDiscountViewModel;
            vm.IsChecked=  !vm.IsChecked;
            var radioButton= (RadioButton)sender;
            if(radioButton.IsChecked==true)
                radioButton.IsChecked= false;
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            InsertDiscountViewModel vm = this.DataContext as InsertDiscountViewModel;
            vm.IsChecked = false;
        }

        private void radiobtn_Click(object sender, RoutedEventArgs e)
        {
            InsertDiscountViewModel vm = this.DataContext as InsertDiscountViewModel;
            vm.IsChecked = !vm.IsChecked;
            if((string)this.radiobtn.Content=="Specific Book")
                this.radiobtn.Content = "All Books";
            else
                this.radiobtn.Content = "Specific Book";
        }
    }
}
