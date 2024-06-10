using BookMK.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.ViewModels.ViewForm
{
    public class ViewOrderViewModel: ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ViewCustomerViewModel));
        private Order _currentorder = new Order();
        public Order CurrentOrder
        {
            get => _currentorder;
            set { _currentorder = value; OnPropertyChanged(nameof(CurrentOrder)); }
        }
        private ObservableCollection<OrderItem> _orderitemlist;
        public ObservableCollection<OrderItem> OrderItemList
        {
            get { return _orderitemlist; }
            set { _orderitemlist = value; OnPropertyChanged(nameof(OrderItemList)); }
        }
        public ViewOrderViewModel(Order i)
        {
            _logger.Information("ViewAuthorViewModel constructor with Order {a} parameter called.", i.ID);
            CurrentOrder = i;

        }
    }
}
