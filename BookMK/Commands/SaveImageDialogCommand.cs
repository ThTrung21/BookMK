using BookMK.ViewModels;
using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace BookMK.Commands
{
    public class SaveImageDialogCommand : AsyncCommandBase
    {
         readonly StringBuilder filename;
         readonly ViewModelBase vm;
        public SaveImageDialogCommand(StringBuilder filename, ViewModelBase vm)
        {

            this.filename = filename;
            this.vm = vm;
        }

        public async override Task ExecuteAsync(object parameter)
        {

            string filePath = null;
            try
            {
                await Task.Run(() =>
                {

                    // Create a SaveFileDialog instance
                    OpenFileDialog saveFileDialog = new OpenFileDialog();
                    saveFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                    // Show the Save File dialog
                    bool? result = saveFileDialog.ShowDialog();

                    // Check if the user selected a file
                    if (result == true)
                    {
                        filePath = saveFileDialog.FileName;

                        // Check if the selected file is valid
                        if (!File.Exists(filePath))
                        {
                            MessageBox.Show("File invalid!!!", "Invalid File", MessageBoxButton.OK, MessageBoxImage.Warning);
                            filePath = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show("File not selected!!!", "File Not Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    if (filePath != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            filename.Clear();
                            filename.Append(filePath);
                            vm.OnPropertyChanged("Filename");
                        });
                    }
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
