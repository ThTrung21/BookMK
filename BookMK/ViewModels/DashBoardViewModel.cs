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

        //Constructor
        public DashBoardViewModel(MainViewModel currentMain)
        {

        }
        public DashBoardViewModel()
        {

        }
        // Navigations Methods
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



    }
}
