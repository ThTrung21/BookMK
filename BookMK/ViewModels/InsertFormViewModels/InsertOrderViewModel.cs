using BookMK.Commands;
using BookMK.Commands.InsertCommand;
using BookMK.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookMK.ViewModels.InsertFormViewModels
{
    public class InsertOrderViewModel: ViewModelBase
    {
        private Book _selectedBook;
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value; OnPropertyChanged(nameof(SelectedBook));
            }
        }
        private Staff _cashier;
        public Staff Cashier
        {
            get { return _cashier; }
            set
            {
                _cashier=value; OnPropertyChanged(nameof(Cashier));
            }
        }

        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }
        private int _amountinput;
        public int AmountInput
        {
            get { return _amountinput; }
            set { _amountinput = value; OnPropertyChanged(nameof(AmountInput)); }
        }
        public ObservableCollection<Book> ComboBoxBooks { get; set; } = new ObservableCollection<Book>(Book.GetBooksList());

        private double _totalprice;
        public double TotalPrice
        {
            get { return _totalprice; }
            set { _totalprice = value; OnPropertyChanged(nameof(TotalPrice)); }
        }

        private double _finalprice ;
        public double FinalPrice
        {
            get { return _finalprice; }
            set { _finalprice = value; OnPropertyChanged(nameof(FinalPrice)); }
        }
        private ObservableCollection<OrderItem> _orderitemlist;
        public ObservableCollection<OrderItem> OrderItemList
        {
            get { return _orderitemlist; }
            set
            {
                _orderitemlist = value;
                OnPropertyChanged(nameof(OrderItemList));
            }
        }
        

        

        //get BOGO discount
        public static List<Discount> GetBOGODiscount()
        {
            DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
            FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq(b => b.Type, "BOGO");
            List<Discount> AllBooks = db.ReadFiltered(filter);

            return AllBooks;
        }
        //--------------------------------------------------------------------------------------------------------------
        //get disocunt for books
        public static List<Discount> GetPercentageDiscount()
        {
            DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
            FilterDefinition<Discount> filter = Builders<Discount>.Filter.And(
                Builders<Discount>.Filter.Eq(x=>x.Type,"Percentage"),
                Builders<Discount>.Filter.Ne(x=>x.BookID,0)
                );
        
            List<Discount> AllBooks = db.ReadFiltered(filter);

            return AllBooks;
        }

        //adding item to list
        public ICommand AddItemCommand => new RelayCommand(AddItem);
        private void AddItem()
        {
            if (SelectedBook==null|| AmountInput == 0)
            {
                MessageBox.Show("Error! Please check your inputs", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedBook.Stock <1)
            {
                MessageBox.Show($"{SelectedBook.Title} is out of stock. Cannot add to order.", "Stock Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (AmountInput >SelectedBook.Stock)
            {
                MessageBox.Show("The amount of item is too much! Cannot add to import.", "Stock Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            

            // Create a new ImportItem and add it to the ObservableCollection
            OrderItem newItem = new OrderItem
            {
                SellBook = this.SelectedBook.Title,
                BookID = this.SelectedBook.ID,
                isGifted = false,
                Quantity = this.AmountInput,
                ItemPrice = SelectedBook.SellPrice * this.AmountInput
            };

            //handle percentage off (if any)
            List<Discount> d = GetPercentageDiscount();
            if (d != null) {
                foreach (Discount a in d)
                {
                    if (a.BookID == newItem.BookID)
                        newItem.ItemPrice = (double)(newItem.ItemPrice -((newItem.ItemPrice / 100) * a.Value));
                }
            }

            //checkstock
            {
                Book currentbook = Book.GetBook(newItem.BookID);
                if (currentbook.Stock - newItem.Quantity < 0)
                {
                    MessageBox.Show($"{currentbook.Title} stock isn't enough for your purchase", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }




            this.TotalPrice += newItem.ItemPrice;
            OrderItemList.Add(newItem);

            //handle Bogo promotion (if any)
            OrderItem giftedbook;
            List<Discount> b = GetBOGODiscount();
            if (b != null)
            {
                foreach (Discount a in b)
                {
                    if (a.BookID == newItem.BookID)
                    {
                        giftedbook = new OrderItem
                        {
                            SellBook = Book.GetBook(a.BookID_free).Title,
                            BookID = a.BookID_free,
                            isGifted = true,
                            Quantity = newItem.Quantity,
                            ItemPrice = 0
                        };

                        //update gifted book stock
                        Book currentbook = Book.GetBook(giftedbook.BookID);
                        if (currentbook.Stock - giftedbook.Quantity < 0)
                        {
                            MessageBox.Show($"{currentbook.Title} stock isn't enough to be fully gifted", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            giftedbook = new OrderItem
                            {
                                SellBook = Book.GetBook(a.BookID_free).Title,
                                BookID = a.BookID_free,
                                isGifted = true,
                                Quantity = currentbook.Stock,
                                ItemPrice = 0
                            };
                            
                        }
                        
                        OrderItemList.Add(giftedbook);
                        
                    }
                }
            }

            this.AmountInput = 0;           
            this.SelectedBook = null;






            this.FinalPrice = TotalPrice;
        }

        public ICommand RemoveItemCommand => new RelayCommand<OrderItem>(RemoveItem);
        private void RemoveItem(OrderItem item)
        {

            
           
            int index = OrderItemList.IndexOf(item);

            // Check if the removed item is not the last one
            while (index < OrderItemList.Count - 1)
            {
                // Get the item right behind the removed item
                OrderItem nextItem = OrderItemList[index + 1];

                // Check if the next item is marked as gifted
                if (nextItem.isGifted)
                {
                    // Remove the next item
                    OrderItemList.Remove(nextItem);
                }
                else
                {
                    // If the next item is not marked as gifted, break the loop
                    break;
                }
            }

            // Remove the original item
            this.TotalPrice -= item.ItemPrice;
            OrderItemList.Remove(item);
        }
        //--------------------------------------------------------------------------------------------------------------

        

        
        public ObservableCollection<Discount> ComboBoxDiscounts { get; set; } = new ObservableCollection<Discount>(Discount.GetDiscountAmountList());

        public ObservableCollection<Customer> ComboBoxCustomer { get; set; } = new ObservableCollection<Customer>(Customer.GetCustomerList());
        
        
        private Discount _selecteddiscount;
        public Discount SelectedDiscount
        {
            get { return _selecteddiscount; }
            set
            {
                _selecteddiscount = value;UpdateTotal(); OnPropertyChanged(nameof(SelectedDiscount));
            }
        }
        private Customer _selectedcustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedcustomer; }
            set
            {
                _selectedcustomer = value; UpdateLoyal();
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }
        public bool loyaldiscountflag;

        public void UpdateTotal()
        {

            if (_selecteddiscount != null)
            {
                if (SelectedDiscount.EligibleBill < TotalPrice)
                {
                    if (FinalPrice < 0)
                        FinalPrice = 0;
                    FinalPrice = TotalPrice - SelectedDiscount.Value;
                }
                else
                {
                    MessageBox.Show("This discount can only applied to bill of " + SelectedDiscount.EligibleBill + "$ and up", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    SelectedDiscount = null;
                    return;
                }

            }
           
        }
       

        public void UpdateLoyal()
        {
            DataProvider<Discount> dbdiscount = new DataProvider<Discount>(Discount.Collection);
            FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq(x => x.ID, 0);
            List<Discount>b=dbdiscount.ReadFiltered(filter);
            Discount loyald = b[0];

            if (_selectedcustomer == null)
                return;

            if (SelectedCustomer.IsLoyalDiscountReady && loyaldiscountflag==false)
            {
                FinalPrice=TotalPrice - loyald.Value;
                if (FinalPrice < 0)
                    FinalPrice = 0;
                loyaldiscountflag=true;
                
            }
            if(!SelectedCustomer.IsLoyalDiscountReady && loyaldiscountflag==true)
            {
                FinalPrice = TotalPrice;
                loyaldiscountflag=false;
            }
            
            
        }

        



        public void UpdateListItem(ObservableCollection<OrderItem> results)
        {
            this.OrderItemList.Clear();
            foreach (OrderItem s in results)
            {
                OrderItemList.Add(s);
            }
        }


        public InsertOrderViewModel()
        {
            

        }
        public InsertOrderViewModel(Staff s)
        {
            this.Cashier = s;
            
            
            loyaldiscountflag=false;
            this.OrderItemList = new ObservableCollection<OrderItem>();
            this.InsertOrder = new InsertOrderCommand(this, Cashier);
        }
        public ICommand InsertOrder { get;set; }
        //public static async Task<InsertOrderViewModel> Initialize()
        //{
        //    InsertOrderViewModel viewModel = new InsertOrderViewModel();
        //    await viewModel.IntializeAsync();
        //    return viewModel;
        //}
        //private async Task IntializeAsync()
        //{
        //    await Task.Run(async () =>
        //    {
        //        // Simulate an asynchronous operation
        //        await Task.Delay(1000);
                
                
        //    });

        //}

    }
}
