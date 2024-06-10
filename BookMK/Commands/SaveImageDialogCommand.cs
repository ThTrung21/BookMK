using BookMK.ViewModels;
using Microsoft.Win32;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands
{
    public class SaveImageDialogCommand : AsyncCommandBase
    {
        private readonly StringBuilder _filename;
        private readonly ViewModelBase _vm;
        private static readonly ILogger _logger = Log.ForContext(typeof(SaveImageDialogCommand));

        public SaveImageDialogCommand(StringBuilder filename, ViewModelBase vm)
        {
            _filename = filename;
            _vm = vm;
            _logger.Information("SaveImageDialogCommand instantiated.");
        }

        public override async Task ExecuteAsync(object parameter)
        {
            string filePath = null;
            try
            {
                await Task.Run(() =>
                {
                    OpenFileDialog saveFileDialog = new OpenFileDialog();
                    saveFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                    bool? result = saveFileDialog.ShowDialog();

                    if (result == true)
                    {
                        filePath = saveFileDialog.FileName;

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
                            _filename.Clear();
                            _filename.Append(filePath);
                            _vm.OnPropertyChanged("Filename");
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Error(ex, "An error occurred while selecting a file.");
            }
        }
    }
}
