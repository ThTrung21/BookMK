using BookMK.Commands.InsertCommand;
using BookMK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookMK.ViewModels.InsertFormViewModels
{
    public class InsertStaffViewModel: ViewModelBase
    {
        
        private Int64 _id;
        public Int64 ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }
        private string _fullname;
        public string FullName
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        private string _phone;
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }



        public ICommand InsertStaff { get; set; }
        
        public InsertStaffViewModel() { }

        //view detail form
        //public InsertStaffViewModel(Staff s)
        //{
        //    this.CurrentStaff = s;
        //    this.Filename.Clear();
        //    this.Filename.Append(ImageStorage.GetImage(ImageStorage.StaffImageLocation, s.Avatar));
        //    OnPropertyChanged(nameof(Filename));
        //}

        public static async Task<InsertStaffViewModel> Initialize()
        {
            InsertStaffViewModel viewModel = new InsertStaffViewModel();
            await viewModel.IntializeAsync();
            return viewModel;
        }
        private async Task IntializeAsync()
        {
            await Task.Run(async () =>
            {
                // Simulate an asynchronous operation
                await Task.Delay(1000);

                ID = Staff.CreateID();
                InsertStaff = new InsertStaffCommand(this);
            });

        }
    }
}
