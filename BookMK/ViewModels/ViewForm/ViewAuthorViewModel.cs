using BookMK.Commands.UpdateCommand;
using BookMK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookMK.ViewModels.ViewForm
{
    public class ViewAuthorViewModel: ViewModelBase
    {
        private Author _currentauthor = new Author();
        public Author CurrentAuthor
        {
            get => _currentauthor;
            set { _currentauthor = value; OnPropertyChanged(nameof(CurrentAuthor)); }
        }

        

        private Int32 _id;
        public Int32 ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }
        private string _name ;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(CurrentAuthor.Name)); }
        }
        private string _note ;
        public string Note
        {
            get { return _note; }
            set { _note = value; OnPropertyChanged(nameof(CurrentAuthor.Note)); }
        }
        
        public ICommand UpdateAuthor { get;set; }

        public ViewAuthorViewModel()
        {
            

        }
        public ViewAuthorViewModel(Author a)
        {
            this.CurrentAuthor = a;
            UpdateAuthor = new UpdateAuthorCommand(this);
        }
    }
}
