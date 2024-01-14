using BookMK.Models;
using BookMK.ViewModels.InsertFormViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for InsertBookForm.xaml
    /// </summary>
    public partial class InsertBookForm : Window
    {
       
        
       
        public InsertBookForm()
        {
            InitializeComponent();
            this.DataContext = new InsertBookViewModel();

            
        }
        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }

        private void CloseBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = await InsertBookViewModel.Initialize();
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertBookViewModel vm = this.DataContext as InsertBookViewModel;
            vm.SelectedGenres= new List<string>();
            foreach (var item in SelectedListBox.Items)
            {
                
                vm.SelectedGenres.Add(item.ToString());
            }

            if (vm.InsertBook != null)
            {
                vm.InsertBook.Execute(this);
            }
        }

       
        private void MoveItems(ListBox source, ListBox destination)
        {
            List<string> selectedItems = new List<string>();

            foreach (var selectedItem in source.SelectedItems)
            {
                selectedItems.Add(selectedItem.ToString());
            }
            bool f = false;
            foreach (var item in selectedItems)
            {
                foreach( var temp in destination.Items)
                {
                    if (item == temp.ToString())
                    {
                        MessageBox.Show(item.ToString() + " already existed");
                        f = true;        
                    }
                }
                if (f)
                    continue;

                destination.Items.Add(item);
            }

        }
        

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedListBox.Items.Count < 3)
                MoveItems(this.AvailableListBox, this.SelectedListBox);
            else
                MessageBox.Show("Maximum genre has been chosen!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void RemoveGenre_Click(object sender, RoutedEventArgs e)
        {
            if(this.SelectedListBox.Items.Count>1)
            {
                this.SelectedListBox.Items.Remove(this.SelectedListBox.SelectedItem);
            }
            else
            {
                MessageBox.Show("Please add another genre before removing this!!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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
    }
}
