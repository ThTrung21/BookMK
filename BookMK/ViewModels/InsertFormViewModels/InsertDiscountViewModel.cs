using BookMK.Commands.InsertCommand;
using BookMK.Commands;
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
    public class InsertDiscountViewModel: ViewModelBase
    {
        public List<Book> booklist;
        private Int32 _id;
        public Int32 ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }
        public ObservableCollection<Book> ComboBoxItems { get; set; } = new ObservableCollection<Book>(Book.GetBooksList());


        public bool IsChecked { get; set; }

        public ObservableCollection<Book> ComboBoxItemsPercentage { get; set; } = new ObservableCollection<Book>(Book.GetBooksList());

        private Book _selectedbasebooks;
        public Book SelectedBaseBooks
        {
            get => _selectedbasebooks;
            set
            {
                _selectedbasebooks = value; OnPropertyChanged(nameof(SelectedBaseBooks));
            }
        }
        private Book _selectedfreebook;
        public Book SelectedFreeBooks
        {
            get => _selectedfreebook;
            set
            {
                _selectedfreebook = value; OnPropertyChanged(nameof(SelectedFreeBooks));
            }
        }

        private int _value;
        public int Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(nameof(Value)); }
        }

        private int _eligible;
        public int EligibleBill
        {
            get { return _eligible; }
            set { _eligible = value; OnPropertyChanged(nameof(EligibleBill)); }
        }


        public ICommand InsertDiscount1 { get; set; }
        public ICommand InsertDiscount2 { get; set; }
        public ICommand InsertDiscount3 { get; set; }
        public static async Task<InsertDiscountViewModel> Initialize()
        {
            InsertDiscountViewModel viewModel = new InsertDiscountViewModel();
            await viewModel.IntializeAsync();
            return viewModel;
        }
        private async Task IntializeAsync()
        {
            await Task.Run(async () =>
            {
                // Simulate an asynchronous operation
                await Task.Delay(1000);





                ID = Discount.CreateID();
     
                InsertDiscount1 = new InsertDiscountCommand(this,1);
                InsertDiscount2 = new InsertDiscountCommand(this,2);
                InsertDiscount3 = new InsertDiscountCommand(this,3);
                
            });

        }
    }
}
