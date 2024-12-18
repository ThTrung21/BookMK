﻿using BookMK.Commands.InsertCommand;
using BookMK.Commands;
using BookMK.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Serilog;

namespace BookMK.ViewModels.InsertFormViewModels
{
    public class InsertDiscountViewModel: ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(InsertDiscountViewModel));
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
        public Book SelectedFreeBook
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

        private int _eligible;
        public int EligibleBill
        {
            get { return _eligible; }
            set { _eligible = value; OnPropertyChanged(nameof(EligibleBill)); }
        }

        public InsertDiscountViewModel()
        {
            _logger.Information("InsertDiscountViewModel constructor called.");
            IsChecked = false;
            InsertDiscount1 = new InsertDiscountCommand(this, 1);
            InsertDiscount2 = new InsertDiscountCommand(this, 2);
            InsertDiscount3 = new InsertDiscountCommand(this, 3);
        }
        public ICommand InsertDiscount1 { get; set; }
        public ICommand InsertDiscount2 { get; set; }
        public ICommand InsertDiscount3 { get; set; }
        public static async Task<InsertDiscountViewModel> Initialize()
        {
            _logger.Information("InsertDiscountViewModel initialization started.");
            InsertDiscountViewModel viewModel = new InsertDiscountViewModel();
            await viewModel.IntializeAsync();
            _logger.Information("InsertDiscountViewModel initialization completed.");
            return viewModel;
        }
        private async Task IntializeAsync()
        {
            _logger.Information("Starting asynchronous initialization of InsertDiscountViewModel.");
            try
            {
                await Task.Run(async () =>
                {
                    // Simulate an asynchronous operation
                    await Task.Delay(1000);
                    IsChecked = false;
                    ID = Discount.CreateID();
                    InsertDiscount1 = new InsertDiscountCommand(this, 1);
                    InsertDiscount2 = new InsertDiscountCommand(this, 2);
                    InsertDiscount3 = new InsertDiscountCommand(this, 3);
                    _logger.Information("Asynchronous initialization of InsertDiscountViewModel completed.");
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred during the asynchronous initialization of InsertDiscountViewModel.");
            }
        }
    }
}
