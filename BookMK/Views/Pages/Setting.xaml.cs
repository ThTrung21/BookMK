using BookMK.Models;
using BookMK.ViewModels;
using BookMK.ViewModels.InsertFormViewModels;
using BookMK.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        bool VerifyBtnEnable = false;
    

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
                   this.Change_Password.Visibility = Visibility.Collapsed;
                }
                if(s.Role!="admin")
                {
                    this.LoyalCustomer.Visibility = Visibility.Collapsed;
                }
            }
            

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //kind: 1-staff
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

        private void discountbtn_Click(object sender, RoutedEventArgs e)
        {
            SettingViewModel vm = this.DataContext as SettingViewModel;

            if (vm.UpdateLoyalDiscount != null)
            {
                vm.UpdateLoyalDiscount.Execute(this);

            }
            
        }

        private void VerifyBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingViewModel vm = this.DataContext as SettingViewModel;
            if (VerifyBtnEnable == false)
            {
                MessageBox.Show("You cant verify the code yet","Warning",MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {

                bool check = vm.CheckCode();
                if(check)
                {
                    this.Change_Password.Visibility = Visibility.Visible;
                }
            }
        }
        public bool IsValidEmail(string email)
        {
            // Regular expression for a basic email format check
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

            // Use Regex.IsMatch to check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }

        private void SendEmailBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingViewModel vm = this.DataContext as SettingViewModel;
            if (String.IsNullOrEmpty(this.Email.Text))
            {
                MessageBox.Show("Please fill in Email first");
                return;
            }
            if (!IsValidEmail(this.Email.Text))
            {
                MessageBox.Show("EMAIL format is incorrect!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (VerifyBtnEnable == false)
            {
                vm.ConfirmMail();
                VerifyBtnEnable = true;
                MessageBox.Show("Code sent");
            }
            else
            {
                MessageBoxResult asking = MessageBox.Show("Are you sure you want to resend the code?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (asking == MessageBoxResult.Yes)
                {
                    vm.ConfirmMail();
                    VerifyBtnEnable = true;
                }
                else
                    return;

            }
        }
    }
}
