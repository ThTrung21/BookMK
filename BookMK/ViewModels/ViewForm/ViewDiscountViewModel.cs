using BookMK.Commands.DeleteCommand;
using BookMK.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookMK.ViewModels.ViewForm
{
    public class ViewDiscountViewModel: ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ViewDiscountViewModel));
        private Discount _currentdiscount = new Discount();
        public Discount CurrentDiscount 
        {
            get { return _currentdiscount; }
            set { _currentdiscount = value; OnPropertyChanged(nameof(CurrentDiscount)); }
        }
        private Int32 _id;
        public Int32 ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }
        private string _type;
        public string Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged(nameof(Type)); }
        }
        private string _selectedbasebooks;
        public string SelectedBaseBooks
        {
            get => _selectedbasebooks;
            set
            {
                _selectedbasebooks = value; OnPropertyChanged(nameof(SelectedBaseBooks));
            }
        }
        private string _selectedfreebook;
        public string SelectedFreeBook
        {
            get => _selectedfreebook;
            set
            {
                _selectedfreebook = value; OnPropertyChanged(nameof(SelectedFreeBook));
            }
        }
        private int _value;
        public int Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(nameof(Value)); }
        }
        private int _eligiblebill;
        public int EligibleBill
        {
            get { return _eligiblebill; }
            set { _eligiblebill = value; OnPropertyChanged(nameof(EligibleBill)); }
        }



        public ICommand DeleteDIscount { get; set; }

        public ViewDiscountViewModel()
        {
            _logger.Information("ViewDiscountViewModel constructor called.");
        }
        public ViewDiscountViewModel(Discount d)
        {
            _logger.Information("ViewDiscountViewModel constructor with discount {a} parameter called.", d.ID);
            this.CurrentDiscount = d;

            if (CurrentDiscount.BookID == 0)
            {
                SelectedBaseBooks = "All books";
            }
            else SelectedBaseBooks = Book.GetBook(CurrentDiscount.BookID).Title;
            
            
            if( CurrentDiscount.BookID_free==-1)
            {
                SelectedFreeBook = null;
            }
            else if(CurrentDiscount.ID==0)
            {
                SelectedFreeBook = null;
            }
            else
                SelectedFreeBook = Book.GetBook(CurrentDiscount.BookID_free).Title;

            DeleteDIscount = new DeleteDiscountCommand(this);
        }
    }
}
