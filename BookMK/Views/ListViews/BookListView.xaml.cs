using System;
using BookMK.Models;
using BookMK.ViewModels;
using BookMK.Views;
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

namespace BookMK.Views.ListViews
{
    /// <summary>
    /// Interaction logic for BookListView.xaml
    /// </summary>
    public partial class BookListView : UserControl
    {
        public BookListView()
        {
            InitializeComponent();
        }
        public ICommand SelectCommand
        {
            get { return (ICommand)GetValue(SelectCommandProperty); }
            set { SetValue(SelectCommandProperty, value); }
        }
        public static readonly DependencyProperty SelectCommandProperty =
           DependencyProperty.Register("SelectCommand", typeof(ICommand), typeof(BookListView), new PropertyMetadata(null));

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectCommand != null)
            {
                SelectCommand.Execute(sender);
            }

        }
    }
}
