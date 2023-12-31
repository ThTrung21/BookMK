﻿using BookMK.Models;
using BookMK.ViewModels.InsertFormViewModels;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    


    public class UpdateAuthorCommand : AsyncCommandBase
    {
        ViewAuthorViewModel vm;

        public UpdateAuthorCommand(ViewAuthorViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Author updatedAuthor = vm.CurrentAuthor;

            try
            {
                // Check if both Name and Note are empty
                if (string.IsNullOrEmpty(updatedAuthor.Name) && string.IsNullOrEmpty(updatedAuthor.Note))
                {
                    MessageBox.Show("Both Name and Note are empty. No update performed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                await Task.Run(() =>
                {
                    FilterDefinition<Author> filter = Builders<Author>.Filter.Eq(x => x.ID, updatedAuthor.ID);
                    UpdateDefinition<Author> update = Builders<Author>.Update
                        .Set(x => x.Name, updatedAuthor.Name)
                        .Set(x => x.Note, updatedAuthor.Note);

                    DataProvider<Author> db = new DataProvider<Author>(Author.Collection);
                    db.Update(filter, update);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Author updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window f = parameter as Window;
                        f?.Close();
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
