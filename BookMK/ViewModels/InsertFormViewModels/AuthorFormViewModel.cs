using BookMK.Commands.InsertCommand;
using BookMK.Models;
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
        private Int32 _id;
        public Int32 ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }
        private String _name = "";
        public String Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        private String _note = "";
        public String Note
        {
            get { return _note; }
            set { _note = value; OnPropertyChanged(nameof(Note)); }
        }

        public ICommand InsertAuthor { get; set; }

        public static async Task<AuthorFormViewModel> Initialize()
        {
            AuthorFormViewModel viewModel = new AuthorFormViewModel();
            await viewModel.IntializeAsync();
            return viewModel;
        }

        private async Task IntializeAsync()
        {
            await Task.Run(() =>
            {
                ID = Author.CreateID();
                InsertAuthor = new InsertAuthorCommand(this);
            });
        }



       

       
    }
}
