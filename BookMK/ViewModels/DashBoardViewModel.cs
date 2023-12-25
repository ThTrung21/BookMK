using BookMK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.ViewModels
{
    public class DashBoardViewModel:ViewModelBase
    {
        private Uri _currentPage;
        public Uri CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
        }
        private Staff _currentStaff;
        public Staff CurrentStaff
        {
            get => _currentStaff;
            set { _currentStaff = value; OnPropertyChanged(nameof(CurrentStaff)); }
        }
        //Constructor
        public DashBoardViewModel(MainViewModel currentMain)
        {

        }
        public DashBoardViewModel()
        {

        }
        public DashBoardViewModel(Staff s)
        {
            this._currentStaff = s;
            OnPropertyChanged(nameof(CurrentStaff));
            SwitchHomePage();

        }


        // Navigations Methods
        public void SwitchHomePage()
        {
            CurrentPage = new Uri("/Views/Pages/HomePage.xaml", UriKind.Relative);
        }
        public void SwitchBillPage()
        {
           // CurrentPage = new Uri("/Views/Pages/CustomerPage.xaml", UriKind.Relative);
        }
        public void SwitchBookPage()
        {
            CurrentPage = new Uri("/Views/Pages/BookPage.xaml", UriKind.Relative);
        }
        public void SwitchAuthorPage()
        {
            CurrentPage = new Uri("/Views/Pages/AuthorPage.xaml", UriKind.Relative);
        }


        public void SwitchCustomerPage()
        {
            CurrentPage = new Uri("/Views/Pages/CustomerPage.xaml", UriKind.Relative);
        }
        
        public void SwitchStaffPage()
        {
            CurrentPage = new Uri("/Views/Pages/StaffPage.xaml", UriKind.Relative);
        }
        public void SwitchSettingPage()
        {
            CurrentPage = new Uri("/Views/Pages/Setting.xaml", UriKind.Relative);
        }
    }

    
}
