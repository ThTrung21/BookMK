using BookMK.Commands.InsertCommand;
using BookMK.Commands.UpdateCommand;
using BookMK.Models;
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
    public class AuthorFormViewModel : ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(AuthorFormViewModel));

        private int _id;
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
               
            }
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
               
            }
        }

        private string _note = "";
        public string Note
        {
            get { return _note; }
            set
            {
                _note = value;
                OnPropertyChanged(nameof(Note));
           
            }
        }

        public AuthorFormViewModel()
        {
            _logger.Information("AuthorFormViewModel constructor called.");
            InsertAuthor = new InsertAuthorCommand(this);
        }

        public ICommand InsertAuthor { get; set; }

        public static async Task<AuthorFormViewModel> Initialize()
        {
           
            AuthorFormViewModel viewModel = new AuthorFormViewModel();
            await viewModel.InitializeAsync();
            _logger.Information("AuthorFormViewModel initialization completed.");
            return viewModel;
        }

        private async Task InitializeAsync()
        {
            _logger.Information("Starting asynchronous initialization of AuthorFormViewModel.");
            try
            {
                await Task.Run(async () =>
                {
                    // Simulate an asynchronous operation
                    await Task.Delay(1000);
                    ID = Author.CreateID();
                    InsertAuthor = new InsertAuthorCommand(this);
                    _logger.Information("Asynchronous initialization of AuthorFormViewModel completed.");
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred during the asynchronous initialization of AuthorFormViewModel.");
            }
        }
    }
}
