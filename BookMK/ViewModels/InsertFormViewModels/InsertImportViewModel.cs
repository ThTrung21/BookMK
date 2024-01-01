using BookMK.Commands;
using BookMK.Commands.InsertCommand;
using BookMK.Models;
using BookMK.Views.InsertForm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BookMK.ViewModels.InsertFormViewModels
{
    public class InsertImportViewModel : ViewModelBase
    {

        //for the textblock fields
        private Book _selectedBook;
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value; OnPropertyChanged(nameof(SelectedBook));
            }
        }
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
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

        private int _amountinput;
        public int AmountInput
        {
            get { return _amountinput; }
            set { _amountinput = value; OnPropertyChanged(nameof(AmountInput)); }
        }
        private double _unitpriceinput;
        public double UnitPriceInput
        {
            get { return _unitpriceinput; }
            set { _unitpriceinput = value; OnPropertyChanged(nameof(UnitPriceInput)); }
        }


       


        private double _totalprice;
        public double TotalPrice
        {
            get { return _totalprice; }
            set { _totalprice = value; OnPropertyChanged(nameof(TotalPrice)); }
        }

        private ObservableCollection<ImportItem> _importItemList;
        public ObservableCollection<ImportItem> ImportItemList
        {
            get { return _importItemList; }
            set
            {
                _importItemList = value;
                OnPropertyChanged(nameof(ImportItemList));
            }
        }

        
        public ICommand AddItemCommand => new RelayCommand(AddItem);
        private void AddItem()
        {
            if (SelectedBook == null || AmountInput == 0 || UnitPriceInput == 0)
            {
                MessageBox.Show("Error! Please check your inputs", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(SelectedBook.Stock >200 )
            {
                MessageBox.Show($"{SelectedBook.Title} still has too much in stock. Cannot add to import.", "Stock Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(AmountInput >200 )
            {
                MessageBox.Show("The amount of item is too much! Cannot add to import.", "Stock Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create a new ImportItem and add it to the ObservableCollection
            ImportItem newItem = new ImportItem
            {
                ImportBook = this.SelectedBook.Title,
                BookID=this.SelectedBook.ID,
                UnitPrice = this.UnitPriceInput,
                Amount = this.AmountInput,
                ItemPrice = this.UnitPriceInput * this.AmountInput
            };
            this.TotalPrice += newItem.ItemPrice;
            ImportItemList.Add(newItem);


            this.AmountInput = 0;
            this.UnitPriceInput = 0;
            this.SelectedBook = null;

        }

        public ICommand RemoveItemCommand => new RelayCommand<ImportItem>(RemoveItem);
        private void RemoveItem(ImportItem item)
        {
            
            this.TotalPrice -= item.ItemPrice;
            ImportItemList.Remove(item);

        }


        public void UpdateListItem(ObservableCollection<ImportItem> results)
        {
            this.ImportItemList.Clear();
            foreach (ImportItem s in results)
            {
                ImportItemList.Add(s);
            }
        }

        public ICommand InsertImport { get;set; }







        public ObservableCollection<Book> ComboBoxBooks { get; set; } = new ObservableCollection<Book>(Book.GetBooksList());


        // public ICommand AddItemCommand { get; }
        public InsertImportViewModel()
        {
            
        }
        public static async Task<InsertImportViewModel> Initialize()
        {
            InsertImportViewModel viewModel = new InsertImportViewModel();
            await viewModel.IntializeAsync();
            return viewModel;
        }
        private async Task IntializeAsync()
        {
            await Task.Run(async () =>
            {
                // Simulate an asynchronous operation
                await Task.Delay(1000);
                ID = Import.CreateID();
                this.ImportItemList = new ObservableCollection<ImportItem>();
                InsertImport = new InsertImportCommand(this);
               
            });

        }
    }
}
