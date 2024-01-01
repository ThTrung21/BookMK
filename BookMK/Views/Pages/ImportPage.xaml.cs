using BookMK.Models;
using BookMK.ViewModels;
using BookMK.Views.InsertForm;
using BookMK.Views.ViewForm;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookMK.Views.Pages
{
    /// <summary>
    /// Interaction logic for ImportPage.xaml
    /// </summary>
    public partial class ImportPage : Page
    {
        public ImportPage()
        {
            InitializeComponent();
           
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertImportForm f = new InsertImportForm();
            f.ShowDialog();
            DataProvider<Import> db = new DataProvider<Import>(Import.Collection);
            List<Import> results = await db.ReadAllAsync();
            (this.DataContext as ImportViewModel).UpdateImportList(results);
        }

        private DateTime _lastClickTime;
        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now - _lastClickTime).TotalMilliseconds < 500)
            {
                Grid g = sender as Grid;
                Import i = g.DataContext as Import;

                ViewImportView f = new ViewImportView(i);
                f.ShowDialog();
            }
            _lastClickTime= DateTime.Now;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = await ImportViewModel.Initialize();
        }

       
    }
}
