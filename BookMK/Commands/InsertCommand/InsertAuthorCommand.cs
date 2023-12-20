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
        AuthorFormViewModel vm;
        public InsertAuthorCommand(AuthorFormViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Int32 ID = vm.ID;
            String name = vm.Name;
            String note = vm.Note;

            if(name == null)
            {
                MessageBox.Show("Please enter an Author's name first!");
                return;
            }
            try
            {
                await Task.Run(async () =>
                {
                    if (Author.IsExisted(name))
                    {
                        MessageBox.Show("This customer already exist, please checkout Name");
                        return;
                    }
                    
                    Author a = new Author()
                    {

                        ID = ID,
                        Name = name,
                        Note = note

                    };
                    DataProvider<Author> db = new DataProvider<Author>(Author.Collection);
                    await db.InsertOneAsync(a);
                    Window f = parameter as Window;
                    f.Close();
                });
            }
            catch
            {

            }
        }
    }
}
