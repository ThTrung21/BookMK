using BookMK.Commands;
using BookMK.Commands.InsertCommand;
using BookMK.Commands.UpdateCommand;
using BookMK.Models;
using BookMK.ViewModels.InsertFormViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ICommand ChangePassword { get; set; }
        public ICommand UpdateLoyalDiscount { get; set; }

        public Discount GetDiscount()
        {
            DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
            FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq(x => x.ID, 0);
            List<Discount> b = db.ReadFiltered(filter);
            if (b.Count() > 0)
                return b[0];
            return null;
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
       


        //public static async Task<SettingViewModel> Initialize(int Kind)
        //{
        //    SettingViewModel viewModel = new SettingViewModel(Kind);
        //    await viewModel.IntializeAsync();
        //    return viewModel;
        //}

        //private async Task IntializeAsync()
        //{
        //    await Task.Run(async () =>
        //    {
        //        // Simulate an asynchronous operation
        //        await Task.Delay(1000);


        //        ChangePassword = new ChangePasswordCommand(this,_kind);
        //    });
        //}
    }
}
