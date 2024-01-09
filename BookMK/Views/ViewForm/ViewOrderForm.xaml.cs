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
    /// Interaction logic for ViewOrderForm.xaml
    /// </summary>
    public partial class ViewOrderForm : Window
    {
        public ViewOrderForm()
        {
            InitializeComponent();
        }
        public ViewOrderForm(Order o)
        {
            InitializeComponent();
            this.DataContext = new ViewOrderViewModel(o);
        }
        private void PackIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
