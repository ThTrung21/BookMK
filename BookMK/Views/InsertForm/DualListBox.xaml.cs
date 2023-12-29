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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookMK.Views.InsertForm
{
    /// <summary>
    /// Interaction logic for DualListBox.xaml
    /// </summary>
    public partial class DualListBox : UserControl
    {
        public List<string> AvailableItems
        {
            get { return (List<string>)AvailableListBox.ItemsSource; }
            set { AvailableListBox.ItemsSource = value; }
        }

        public List<string> SelectedItems
        {
            get { return (List<string>)SelectedListBox.ItemsSource; }
            set { SelectedListBox.ItemsSource = value; }
        }

        public DualListBox()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MoveItems(AvailableListBox, SelectedListBox);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            MoveItems(SelectedListBox, AvailableListBox);
        }

        private void MoveItems(ListBox source, ListBox destination)
        {
            List<string> selectedItems = new List<string>();
            foreach (var selectedItem in source.SelectedItems)
            {
                selectedItems.Add(selectedItem.ToString());
            }

            foreach (var item in selectedItems)
            {
                source.Items.Remove(item);
                destination.Items.Add(item);
            }
        }
    }
}
