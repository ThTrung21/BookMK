using BookMK.Models;
using BookMK.ViewModels.InsertFormViewModels;
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
    /// Interaction logic for ViewStaffForm.xaml
    /// </summary>
    public partial class ViewStaffForm : Window
    {
        Staff s { get; set; }

        public ViewStaffForm()
        {
            InitializeComponent();
        }
        public ViewStaffForm(Staff s)
        {
            InitializeComponent();
           
            this.DataContext = new ViewStaffViewModel(s);
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
            ViewStaffViewModel vm = this.DataContext as ViewStaffViewModel;

            if (vm.UpdateStaff != null)
            {
                vm.UpdateStaff.Execute(this);
            }
        }
        private void CloseBtn_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
