using BookMK.Commands;
using BookMK.Commands.InsertCommand;
using BookMK.Commands.UpdateCommand;
using BookMK.Models;
using BookMK.Service;
using BookMK.ViewModels.InsertFormViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookMK.ViewModels
{
    public class SettingViewModel: ViewModelBase
    {
        private Staff _currentStaff;
        public Staff CurrentStaff
        {
            get => _currentStaff;
            set { _currentStaff = value; OnPropertyChanged(nameof(CurrentStaff)); }
        }
        private Customer _currentCustomer;
        public Customer CurrentCustomer
        {
            get => _currentCustomer;
            set { _currentCustomer = value; OnPropertyChanged(nameof(CurrentCustomer)); }
        }

        private string _newpassword;
        public string NewPassword
        {
            get => _newpassword;
            set { _newpassword = value; OnPropertyChanged(nameof(NewPassword)); }
        }
        private double _discountamount;
        public double DiscountAmount
        {
            get => _discountamount;
            set { _discountamount = value; OnPropertyChanged(nameof(DiscountAmount)); }
        }
        private double _pointmilestone;
        public double PointMilestone
        {
            get => _pointmilestone;
            set { _pointmilestone = value; OnPropertyChanged(nameof(PointMilestone)); }
        }
        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }
        private string _code;
        public string Code
        {
            get => _code;
            set { _code = value; OnPropertyChanged(nameof(Code)); }
        }

        public ICommand ChangePassword { get; set; }
        public ICommand UpdateLoyalDiscount { get; set; }
        public ICommand UpdateStaff { get; set; }
        public ICommand verify { get;set; }
        public Discount GetDiscount()
        {
            DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
            FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq(x => x.ID, 0);
            List<Discount> b = db.ReadFiltered(filter);
            if (b.Count() > 0)
                return b[0];
            return null;
        }

        public bool IsValidEmail(string email)
        {
            // Regular expression for a basic email format check
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

            // Use Regex.IsMatch to check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }
        public string Current6Digits { get; set; }

        public async void ConfirmMail()
        {
            await Task.Run(() =>
            {
                string str = MailService.Generate6Digits();
                Current6Digits = str;
                Email = CurrentStaff.Email;
               

               MailService.SendEmail(Email, "[NoReply] Verify your Email", str);
            });
        }
        public bool CheckCode()
        {
            if (!String.IsNullOrEmpty(Current6Digits) && Code== Current6Digits)
            {
                UpdateStaff = new UpdateAccountCommand(this);
                UpdateStaff.Execute(this);
                return true;
            }
            else
            {
                MessageBox.Show("Your code is incorrect!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }






        public SettingViewModel() { }
        public SettingViewModel(Staff s)
        {
           Discount loyal=GetDiscount();
            this.DiscountAmount = loyal.Value;
            this.PointMilestone = loyal.PointMileStone;


            this.CurrentStaff = s;
            
            this.ChangePassword = new ChangePasswordCommand(this,1);

            this.UpdateLoyalDiscount= new UpdateLoyalDiscountCommand(this);
        }
       


        
    }
}
