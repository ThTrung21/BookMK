using BookMK.Models;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.ViewModels
{
    public class StatisticViewModel : ViewModelBase
    {
        private ObservableCollection<Book> _outofstockbooks;
        public ObservableCollection<Book> OutOfStockBooks
        {
            get { return _outofstockbooks; }
            set { _outofstockbooks = value; OnPropertyChanged(nameof(OutOfStockBooks)); }
        }

        private SeriesCollection _seriescollection;

        public SeriesCollection SeriesCollection
        {
            get { return _seriescollection; }
            set
            {
                _seriescollection = value;
                OnPropertyChanged(nameof(SeriesCollection));
            }
        }
        private List<DateTime> _allDatesInMonth;
        public List<DateTime> AllDatesInMonth
        {
            get { return _allDatesInMonth; }
            set
            {
                _allDatesInMonth = value;
                OnPropertyChanged(nameof(AllDatesInMonth));
            }
        }
        public double TotalMonth { get; set; }

        public StatisticViewModel()
        {
            DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
            List<Book> a =  Book.GetOutOfStock();

            this.OutOfStockBooks = new ObservableCollection<Book>(a);

            //=====================================================================
            //current month revenue
           

           

            SeriesCollection = new SeriesCollection();
            var salesData = GetSalesDataForCurrentMonth();
            AllDatesInMonth = GetDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            foreach ( Order o in salesData)
            {
                TotalMonth += o.Total;
            }

            //var dayRevenuePairs = salesData
            //.GroupBy(order => order.Time.Day)
            //.Select(group => new { Day = group.Key, Revenue = group.Sum(order => order.Total) })
            //.OrderBy(pair => pair.Day);
            var dayRevenuePairs = AllDatesInMonth
            .Select(day => new
            {
                Day = day.Day,
                Revenue = salesData.Where(order => order.Time.Day == day.Day).Sum(order => order.Total)
            });
            var barSeries = new ColumnSeries
            {
                Title = "Revenue: ",
                Values = new ChartValues<ObservablePoint>(
                    dayRevenuePairs.Select(pair => new ObservablePoint(pair.Day, pair.Revenue))
                )
            };

            // Add the series to the chart
            SeriesCollection.Add(barSeries);




        }
        private List<DateTime> GetDaysInMonth(int year, int month)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            return Enumerable.Range(1, daysInMonth)
                .Select(day => new DateTime(year, month, day))
                .ToList();
        }
        private List<Order> GetSalesDataForCurrentMonth()
        {
            DataProvider<Order> db = new DataProvider<Order>(Order.Collection);

            // Get the first day of the current month
            DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Get the first day of the next month
            DateTime startOfNextMonth = startOfMonth.AddMonths(1);

            // Filter orders within the current month
            FilterDefinition<Order> filter = Builders<Order>.Filter.And(
                   Builders<Order>.Filter.Gte(x => x.Time, startOfMonth),    // Greater than or equal to the start of the month
                 Builders<Order>.Filter.Lt(x => x.Time, startOfNextMonth));  // Less than the start of the next month

            List<Order> ordersInCurrentMonth = db.ReadFiltered(filter);

            return ordersInCurrentMonth;

          
        }

    }
}
