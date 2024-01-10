using BookMK.Models;
using BookMK.ViewModels;
using BookMK.ViewModels.ViewForm;
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
using System.Windows.Shapes;

namespace BookMK.Views.ViewForm
{
    /// <summary>
    /// Interaction logic for ViewBookForm.xaml
    /// </summary>
    public partial class ViewBookForm : Window
    {
        Staff s { get;set; }
        public ViewBookForm()
        {
            InitializeComponent();
        }
        public ViewBookForm(Book b)
        {
            InitializeComponent();
            DashBoardWindow main = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive) as DashBoardWindow;
            DashBoardViewModel vm = main.DataContext as DashBoardViewModel;
            Staff loggedinS = vm.CurrentStaff;
            s = loggedinS;
            if (s.Role != "admin")
            {
                this.InsertBtn.Visibility = Visibility.Collapsed;
                this.DeleteBtn.Visibility = Visibility.Collapsed;
            }
            this.DataContext = new ViewBookViewModel(b);
        }

        private void CloseBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewBookViewModel vm = this.DataContext as ViewBookViewModel;
            if (vm.UpdateBook != null)
            {
                vm.UpdateBook.Execute(this);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewBookViewModel vm = this.DataContext as ViewBookViewModel;
            if (vm.DeleteBook != null)
            {
                vm.DeleteBook.Execute(this);
            }
        }
    }
}
