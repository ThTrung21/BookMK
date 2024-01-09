using BookMK.Models;
using BookMK.ViewModels;
using BookMK.ViewModels.InsertFormViewModels;
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
    /// Interaction logic for InsertImportForm.xaml
    /// </summary>
    public partial class InsertImportForm : Window
    {
        public InsertImportForm()
        {
            InitializeComponent();
            this.DataContext= new InsertImportViewModel();
        }

        private void PackIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }


        private void AddItemBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertImportViewModel vm= this.DataContext as InsertImportViewModel;
           // vm.AddItemToImportList();



        }

        private void InsertBillBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertImportViewModel vm = this.DataContext as InsertImportViewModel;
            if(vm.InsertImport!=null)
            {
                vm.InsertImport.Execute(this);
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = await InsertImportViewModel.Initialize();
        }

       
        private DateTime _lastClickTime;
        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now - _lastClickTime).TotalMilliseconds < 500)
            {
                //var selectedItem = ((ListView)sender).SelectedItem as ImportItem;
                Grid g = sender as Grid;
                ImportItem i = g.DataContext as ImportItem;
                // Check if an item is selected
                if (i != null)
                {
                    
                    // Execute the remove command in the ViewModel
                    ((InsertImportViewModel)DataContext).RemoveItemCommand.Execute(i);
                }
            }
            _lastClickTime = DateTime.Now;
        }
    }
}
