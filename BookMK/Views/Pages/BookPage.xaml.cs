using BookMK.Models;
using BookMK.ViewModels;
using BookMK.Views.InsertForm;
using BookMK.Windows;
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
    /// Interaction logic for BookPage.xaml
    /// </summary>
    public partial class BookPage : Page
    {
        Staff s { get; set; }
        Customer c { get; set; }
        public BookPage()
        {
            InitializeComponent();
            DashBoardWindow main = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive) as DashBoardWindow;
            DashBoardViewModel vm = main.DataContext as DashBoardViewModel;
            Staff loggedinS = vm.CurrentStaff;
            s = loggedinS;
            if(s != null)
            {
                this.AddBtn.Visibility = Visibility.Collapsed;
            }

            ComboboxFilter.ItemsSource = new List<string>() { "All", "Mystery","Romance","Sci-Fi","Thriller","History","Education","Comic", "Fantasy" };
            this.DataContext =new BookViewModel();
        }
       

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            InsertBookForm f = new InsertBookForm();
            f.ShowDialog();
            DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
            List<Book> results = await db.ReadAllAsync();
            (this.DataContext as BookViewModel).UpdateBookList(results);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = await BookViewModel.Initialize();
        }

        
    }
}
