using BookMK.Models;
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
    /// Interaction logic for ViewImportView.xaml
    /// </summary>
    public partial class ViewImportView : Window
    {
        public ViewImportView()
        {
            InitializeComponent();
        }
        public ViewImportView(Import i)
        {
            InitializeComponent();
            this.DataContext = new ViewImportViewModel(i);
        }
        private void PackIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
