﻿using BookMK.Models;

using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.ViewModels
{
    public class AuthorViewModel: ViewModelBase
    {
        public AuthorViewModel() { }

        
        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors
        {
            get { return _authors; }
            set { _authors = value; OnPropertyChanged(nameof(Author)); }
        }


        private string _searchString = "";
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                //Search();
                OnPropertyChanged(nameof(SearchString));
            }
        }


       

        //update list
        public void UpdateAuthorList(List<Author> authors)
        {
            this.Authors.Clear();
            foreach (Author c in authors)
            {
                Authors.Add(c);
            }
        }

        //async
        public static async Task<AuthorViewModel> Initialize()
        {
            AuthorViewModel viewModel = new AuthorViewModel();
            await viewModel.InitializeAsync();
            return viewModel;
        }

        private async Task InitializeAsync()
        {
            DataProvider<Author> db = new DataProvider<Author>(Author.Collection);
            List<Author> AllAuthors = await db.ReadAllAsync();
            this._authors = new ObservableCollection<Author>(AllAuthors);

        }

        //search
        private async void Search()
        {
            await Task.Run(() =>
            {
                DataProvider<Author> db = new DataProvider<Author>(Author.Collection);
                string searchInput = SearchString.Trim();
                List<Author> results = new List<Author>();

                //FilterDefinition<Customer> filter = Builders<Customer>.Filter.Where(c => c.ID.ToString().Contains(searchInput));
                //results = db.ReadFiltered(filter

                FilterDefinition<Author> filter = Builders<Author>.Filter.Regex("Name", new BsonRegularExpression(searchInput, "i"));
                results = db.ReadFiltered(filter);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Authors.Clear();
                    foreach (Author c in results)
                    {
                        Authors.Add(c);
                    }
                });
            });
        }
    }

    

}
