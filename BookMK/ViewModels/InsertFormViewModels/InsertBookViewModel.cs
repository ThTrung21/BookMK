using BookMK.Commands;
using BookMK.Commands.InsertCommand;
using BookMK.Models;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookMK.ViewModels.InsertFormViewModels
{
    public class InsertBookViewModel:ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(InsertBookViewModel));
        public List<Author> authorList;

        private Int32 _id;
        public Int32 ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }
        private string _releaseyear;
        public string ReleaseYear
        {
            get { return _releaseyear; }
            set { _releaseyear = value; OnPropertyChanged(nameof(ReleaseYear)); }
        }
        private double _sellprice;
        public double SellPrice
        {
            get { return _sellprice; }
            set { _sellprice = value; OnPropertyChanged(nameof(SellPrice)); }
        }
        //private string _author;
        //public string AuthorName
        //{
        //    get { return _author; }
        //    set { _author = value; OnPropertyChanged(nameof(Author)); }
        //}

        public ObservableCollection<Author> ComboBoxItems { get; set; } = new ObservableCollection<Author>(Author.GetAuthorsList());

        private StringBuilder _filename = new StringBuilder("");
        public StringBuilder Filename
        {
            get => _filename;
            set { _filename = value; OnPropertyChanged(nameof(Filename)); }
        }

        private Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                _selectedAuthor = value;
                OnPropertyChanged(nameof(SelectedAuthor));
            }
        }
        



        
        public ObservableCollection<string> ComboBoxGenreItems { get; set; } = new ObservableCollection<string>(new List<string>() {  "Mystery", "Romance", "Sci-Fi", "Thriller", "History", "Education", "Comic", "Fantasy" });
        
        private List<string> _selectedGenres;
        public List<string> SelectedGenres
        {
            get { return _selectedGenres; }
            set
            {     
                _selectedGenres = value;               
                OnPropertyChanged(nameof(SelectedGenres));
              
            }
        }




      


        public ICommand SaveImageDialog { get; set; }
        public ICommand InsertBook { get; set; }

        public InsertBookViewModel() 
        {
            _logger.Information("InsertBookViewModel constructor called.");
            this.SaveImageDialog = new SaveImageDialogCommand(Filename, this);
            InsertBook = new InsertBookCommand(this, Filename);
        }

        

        public static async Task<InsertBookViewModel> Initialize()
        {
            InsertBookViewModel viewModel = new InsertBookViewModel();
            await viewModel.IntializeAsync(); _logger.Information("InsertBookViewModel initialization completed.");
            return viewModel;
        }
        private async Task IntializeAsync()
        {
            _logger.Information("Starting asynchronous initialization of InsertBookViewModel.");
            try { 
            await Task.Run(async () =>
            {
                // Simulate an asynchronous operation
                await Task.Delay(1000);





                ID = Book.CreateID();
                //SaveImageDialog = new SaveImageDialogCommand(Filename, this);
                InsertBook = new InsertBookCommand(this,Filename);
                this.SaveImageDialog = new SaveImageDialogCommand(Filename, this);
                _logger.Information("Asynchronous initialization of InsertBookViewModel completed.");
            });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred during the asynchronous initialization of InsertBookViewModel.");
            }
        }

    }
}
