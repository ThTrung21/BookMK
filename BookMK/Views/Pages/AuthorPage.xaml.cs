using System;
using BookMK.ViewModels;
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
using BookMK.Models;
using BookMK.Views.InsertForm;

namespace BookMK.Views.Pages
{
    /// <summary>
    /// Interaction logic for AuthorPage.xaml
    /// </summary>
    public partial class AuthorPage : Page
    {
        public AuthorPage()
        {
            InitializeComponent();
            
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = await AuthorViewModel.Initialize();
           
        }

        private DateTime _lastClickTime;
        private async void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now - _lastClickTime).TotalMilliseconds < 500)
            {
                Grid g = sender as Grid;
                Customer c = g.DataContext as Customer;
                //ViewCustomerForm f = new ViewCustomerForm(c, s);
                //f.ShowDialog();
                if (this.DataContext != null)
                {
                    AuthorViewModel ViewModel = this.DataContext as AuthorViewModel;
                    DataProvider<Author> db = new DataProvider<Author>(Author.Collection);
                    ViewModel.UpdateAuthorList(await db.ReadAllAsync());
                }
            }
            _lastClickTime = DateTime.Now;


        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertAuthorForm f = new InsertAuthorForm();
            f.ShowDialog();
            if (this.DataContext != null)
            {
                AuthorViewModel ViewModel = this.DataContext as AuthorViewModel;
                DataProvider<Author> db = new DataProvider<Author>(Author.Collection);
                ViewModel.UpdateAuthorList(await db.ReadAllAsync());
            }
        }
    }
}
