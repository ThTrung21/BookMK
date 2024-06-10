﻿using BookMK.Models;
using BookMK.ViewModels.InsertFormViewModels;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.InsertCommand
{
    public class InsertAuthorCommand : AsyncCommandBase
    {
        private readonly AuthorFormViewModel vm;

        public InsertAuthorCommand(AuthorFormViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                Int32 ID = vm.ID;
                String _Name = vm.Name;
                String _Note = vm.Note;

                if (string.IsNullOrWhiteSpace(_Name))
                {
                    MessageBox.Show("Please enter an Author's name first!");
                    return;
                }

                if (Author.IsExisted(_Name))
                {
                    MessageBox.Show("This author already exists. Please check the name.");
                    return;
                }

                Author a = new Author()
                {
                    ID = ID,
                    Name = _Name,
                    Note = _Note
                };

                DataProvider<Author> db = new DataProvider<Author>(Author.Collection);
                await db.InsertOneAsync(a);

                MessageBox.Show("Added a new author!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Window f = parameter as Window;
                f?.Close();

                // Log success
                Log.Information("New author added: {AuthorName}", _Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while inserting a new 
