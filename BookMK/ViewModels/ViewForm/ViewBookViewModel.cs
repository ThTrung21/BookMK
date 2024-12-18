﻿using Amazon.Runtime.Internal.Util;
using BookMK.Commands;
using BookMK.Commands.DeleteCommand;
using BookMK.Commands.UpdateCommand;
using BookMK.Models;
using BookMK.Service;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ILogger = Serilog.ILogger;

namespace BookMK.ViewModels.ViewForm
{
    public class ViewBookViewModel: ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ViewBookViewModel));
        private Book _currentBook = new Book();
        public Book CurrentBook
        {
            get => _currentBook;
            set { _currentBook = value; OnPropertyChanged(nameof(CurrentBook)); }
        }
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
        private int _stock;
        public int Stock
        {
            get { return _stock; }
            set { _stock = value; OnPropertyChanged(nameof(Stock)); }
        }
        private string _author;
        public string AuthorName
        {
            get { return _author; }
            set { _author = value; OnPropertyChanged(nameof(Author)); }
        }
        private StringBuilder _filename = new StringBuilder("");
        public StringBuilder Filename
        {
            get => _filename;
            set { _filename = value; OnPropertyChanged(nameof(Filename)); }
        }
        


        public ICommand UpdateBook { get;set; }
        public ICommand SaveImageDialog { get; set; }
        public ICommand DeleteBook { get; set; }

        public ViewBookViewModel() { _logger.Information("ViewBookViewModel constructor called."); }
        public ViewBookViewModel(Book b) 
        {
            _logger.Information("ViewBookViewModel constructor with Book {a} parameter called.",b.ID);
            this.CurrentBook = b;
            this.Filename.Clear();
            this.Filename.Append(ImageStorage.GetImage(ImageStorage.BookImageLocation, b.Cover));
           
            this.SaveImageDialog = new SaveImageDialogCommand(Filename, this);
            this.UpdateBook = new UpdateBookCommand(this, Filename);
            this.DeleteBook = new DeleteBookCommand(this, Filename);

        }
    }
}
