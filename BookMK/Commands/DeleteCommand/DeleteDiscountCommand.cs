using BookMK.Models;
using BookMK.ViewModels.ViewForm;
using MongoDB.Driver;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.DeleteCommand
{
    public class DeleteDiscountCommand : AsyncCommandBase
    {
        private readonly ViewDiscountViewModel _vm;
        private static readonly ILogger _logger = Log.ForContext(typeof(DeleteDiscountCommand));

        public DeleteDiscountCommand(ViewDiscountViewModel vm)
        {
            _vm = vm;
            _logger.Information("DeleteDiscountCommand instantiated.");
        }

        public override async Task ExecuteAsync(object parameter)
        {
            Discount currentDiscount = _vm.CurrentDiscount;

            MessageBoxResult asking = MessageBox.Show("Are you sure you want to DELETE?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (asking == MessageBoxResult.No) return;

            try
            {
                if (currentDiscount.Type == "Loyal")
                {
                    MessageBox.Show("You can't delete this type of discount!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                await Task.Run(() =>
                {
                    FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq(x => x.ID, currentDiscount.ID);
                    DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
                    db.Delete(filter);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Discount deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window f = parameter as Window;
                        f?.Close();
                    });
                });

                _logger.Information("Discount deleted: {DiscountId}", currentDiscount.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Error(ex, "An error occurred while deleting the discount.");
            }
        }
    }
}
