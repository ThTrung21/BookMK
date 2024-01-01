using BookMK.ViewModels.InsertFormViewModels;
using BookMK.Views.InsertForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Commands.InsertCommand
{
    public class AddItemImportCommand
    {
        InsertImportViewModel vm;
        public AddItemImportCommand(InsertImportViewModel vm) 
        {
            this.vm = vm;
        }
        public void Execute()
        {
          
        }
    }
}
