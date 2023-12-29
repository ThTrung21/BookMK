using BookMK.Commands;
using BookMK.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.ViewModels
{
    public class BookViewModel :ViewModelBase
    {
        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set { _books = value; OnPropertyChanged(nameof(Books)); }
        }
        private string _searchString = "";
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                Search();
                OnPropertyChanged(nameof(SearchString));
            }
        }
       
        private int _id;
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private string _title;
        public string Title
        {
            get 
            { 
                return _title;
            }
            set { _title = value; OnPropertyChanged(nameof(Title));}
        }

        private string _cover;
        public string Cover
        {
            get { return _cover; }
            set { _cover = value; OnPropertyChanged(nameof(Cover)); }
        }

        private string _authorf;
        public string AuthorF
        {
            get { return _authorf; }
            set
            {
                _authorf = value; OnPropertyChanged(nameof(AuthorF));
            }



        }

        private string _releaseyear;
        public string ReleaseYear
        {
            get { return _releaseyear; }
            set { _releaseyear = value; OnPropertyChanged(nameof(ReleaseYear)); }
        }


        

        public ObservableCollection<string> ComboBoxItems { get; set; } = new ObservableCollection<string>(new List<string>() { "All", "Mystery", "Romance", "Sci-Fi", "Thriller" ,"History", "Education", "Comic", "Fantasy" });
        private int _selectedIndex = 0;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; Search(); OnPropertyChanged(nameof(SelectedIndex)); }
        }
       




        public BookViewModel()
        {
            
        }
        public void UpdateBookList(List<Book> books)
        {
            this.Books.Clear();
            foreach (Book b in books)
            {
                Books.Add(b);
            }
        }
        public static async Task<BookViewModel> Initialize()
        {
            BookViewModel viewModel = new BookViewModel();
            await viewModel.InitializeAsync();
            return viewModel;
        }
        private async Task InitializeAsync()
        {
            DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
            List<Book> AllBooks = await db.ReadAllAsync();
            this._books = new ObservableCollection<Book>(AllBooks);

        }

        private async void Search()
        {
            await Task.Run(() =>
            {
                DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
                string searchInput = SearchString.Trim().ToLower();
                List<Book> results = new List<Book>();


                if(SelectedIndex == 0)
                {
                    FilterDefinition<Book> filter = Builders<Book>.Filter.Where(
                    b => (b.Title.ToLower().Trim().Contains(searchInput) || b.AuthorName.ToLower().Trim().Contains(searchInput)));
                    results = db.ReadFiltered(filter);
                }
                else
                {
                    FilterDefinition<Book> nameAndGenreFilter = Builders<Book>.Filter.Where(
                    b => b.Title.ToLower().Trim().Contains(searchInput) && b.Genre.Contains(ComboBoxItems[_selectedIndex]));
                    results = db.ReadFiltered(nameAndGenreFilter);
                }



                Application.Current.Dispatcher.Invoke(() =>
                {
                    Task.Delay(500);
                    Books.Clear();
                    foreach (Book b in results)
                    {
                       
                            Books.Add(b);
                    }
                });
            });
        }


    }
}
