using BookMK.Models;
using BookMK.ViewModels.InsertFormViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.InsertCommand
{
    public class InsertDiscountCommand: AsyncCommandBase
    {
        private readonly InsertDiscountViewModel vm;
        private int discounttype;
        public InsertDiscountCommand(InsertDiscountViewModel vm, int discounttype)
        {
            this.vm = vm;
            this.discounttype = discounttype;
        }
        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                int _ID = vm.ID;
                DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
                switch (discounttype)
                {
                    case 1:
                        int bookdiscount;
                        if (vm.IsChecked)
                        {
                            bookdiscount = 0;
                        }
                        else
                            bookdiscount = vm.SelectedBaseBooks.ID;
                        Discount d = new Discount()
                        {
                            ID = _ID,
                            Type = "Percentage",
                            BookID = bookdiscount,
                            BookID_free = -1,
                            EligibleBill=0,
                            Value = vm.Value,
                            Time = DateTime.Now,


                        };
                        await db.InsertOneAsync(d);

                        break;
                    case 2:
                        Discount d1 = new Discount()
                        {
                            ID = _ID,
                            Type = "Amount",
                            BookID = -1,
                            BookID_free = -1,
                            EligibleBill =vm.EligibleBill,
                            Value = vm.Value,
                            Time = DateTime.Now,
                        };
                        await db.InsertOneAsync(d1);
                        break;
                    case 3:
                        Discount d2 = new Discount()
                        {
                            ID = _ID,
                            Type = "BOGO",
                            BookID = vm.SelectedBaseBooks.ID,
                            BookID_free = vm.SelectedFreeBook.ID,
                            EligibleBill = 0,
                            Value = 0,
                            Time = DateTime.Now,
                        };
                        await db.InsertOneAsync(d2);

                        break;

                }
                //initialize the import
               

               




               
                

                MessageBox.Show("A new discount has been created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Window f = parameter as Window;
                f?.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
