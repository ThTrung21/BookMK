using BookMK.Models;
using BookMK.ViewModels;
using BookMK.ViewModels.InsertFormViewModels;
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
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : Page
    {
        Staff s { get; set; }
        Customer c { get; set; }


        public Setting()
        {
            InitializeComponent();
            DashBoardWindow main = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive) as DashBoardWindow;
            DashBoardViewModel vm = main.DataContext as DashBoardViewModel;

            if (vm.CurrentStaff != null)
            {
                Staff loggedinS = vm.CurrentStaff;
                s = loggedinS;
                if (s.IsVerified == false)
                {
                   // this.Change_Password.Visibility = Visibility.Collapsed;
                }
            }
            else
            {

            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //kind: 1-staff 2-customer
            if (s != null)
            {
                this.DataContext = new SettingViewModel(s);
                
            }
            else
            {
                //   this.DataContext = new SettingViewModel(c);
            }
        }

        //change password btn
        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingViewModel vm = this.DataContext as SettingViewModel;

            if (vm.ChangePassword != null)
            {
                vm.ChangePassword.Execute(this);

            }
            this.NewPassword_field.Text = null;
        }
    }
}
