using BookMK.Models;
using BookMK.ViewModels;
using BookMK.ViewModels.InsertFormViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                if (_Name == null)
                {
                    MessageBox.Show("Please enter an Author's name first!");
                    return;
                }

                if (Author.IsExisted(_Name))
                {
                    MessageBox.Show("This customer already exist, please checkout Name");
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

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
