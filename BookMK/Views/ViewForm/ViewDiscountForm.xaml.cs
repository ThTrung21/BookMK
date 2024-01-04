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
    /// Interaction logic for ViewDiscountForm.xaml
    /// </summary>
    public partial class ViewDiscountForm : Window
    {
        public ViewDiscountForm()
        {
            InitializeComponent();
        }
        public ViewDiscountForm(Discount d)
        {
            InitializeComponent();
            this.DataContext = new ViewDiscountViewModel(d);
            ViewDiscountViewModel vm = this.DataContext as ViewDiscountViewModel;
            if (vm.CurrentDiscount.Type=="Percentage")
            {
                this.Amount.Visibility=Visibility.Collapsed;
                this.BOGO.Visibility=Visibility.Collapsed;

            }
            if (vm.CurrentDiscount.Type == "Amount")
            {
                this.Percentage.Visibility = Visibility.Collapsed;
                this.BOGO.Visibility = Visibility.Collapsed;

            }
            if (vm.CurrentDiscount.Type == "BOGO")
            {
                this.Amount.Visibility = Visibility.Collapsed;
                this.Percentage.Visibility = Visibility.Collapsed;

            }


        }
      

        private void PackIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ViewDiscountViewModel vm = this.DataContext as ViewDiscountViewModel;
            if (vm.DeleteDIscount != null)
            {
                vm.DeleteDIscount.Execute(this);
            }
        }
    }
}
