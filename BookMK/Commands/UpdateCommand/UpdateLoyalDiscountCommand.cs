﻿using BookMK.Models;
using BookMK.ViewModels;
using MongoDB.Driver;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace BookMK.Commands.UpdateCommand
{
    internal class UpdateLoyalDiscountCommand : AsyncCommandBase
    {
        private readonly SettingViewModel vm;

        public UpdateLoyalDiscountCommand(SettingViewModel vm)
        {
            this.vm = vm;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await Task.Run(() =>
                {
                    DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
                    FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq("_id", 0);
                    UpdateDefinition<Discount> update = Builders<Discount>.Update
                        .Set(x => x.PointMileStone, vm.PointMilestone)
                        .Set(x => x.Value, vm.DiscountAmount);
                    db.Update(filter, update);

                    MessageBox.Show("Loyal customer discount is updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Window f = parameter as Window;
                    f?.Close();

                    // Log success
                    Log.Information("Loyal customer discount updated successfully: PointMilestone - {PointMilestone}, DiscountAmount - {DiscountAmount}", vm.PointMilestone, vm.DiscountAmount);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Log error
                Log.Error(ex, "Error occurred while updating loyal customer discount: PointMilestone - {PointMilestone}, DiscountAmount - {DiscountAmount}", vm.PointMilestone, vm.DiscountAmount);
            }
        }
    }
}
