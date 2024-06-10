
using BookMK.Models;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookMK.ViewModels
{
    public class StatisticViewModel : ViewModelBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(StatisticViewModel));
        private ObservableCollection<Book> _outOfStockBooks;
        public ObservableCollection<Book> OutOfStockBooks
        {
            get { return _outOfStockBooks; }
            set { _outOfStockBooks = value; OnPropertyChanged(nameof(OutOfStockBooks)); }
        }

        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                OnPropertyChanged(nameof(SeriesCollection));
            }
        }

        private SeriesCollection _seriesPie;
        public SeriesCollection SeriesPie
        {
            get { return _seriesPie; }
            set
            {
                _seriesPie = value;
                OnPropertyChanged(nameof(SeriesPie));
            }
        }

        public List<DateTime> AllDatesInLast30Days { get; set; }
        public int TotalBookMonth { get; set; }

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
        public List<string> XAxisLabels { get; set; }
        public double TotalMonth { get; set; }

        public StatisticViewModel()
        {
            _logger.Information("StatisticViewModel initialization started.");

            try
            {
                DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
                List<Book> outOfStockBooks = Book.GetOutOfStock();
                this.OutOfStockBooks = new ObservableCollection<Book>(outOfStockBooks);
                _logger.Information("Retrieved out of stock books.");

                InitializeSeriesCollection();
                InitializeSeriesPie();

                _logger.Information("StatisticViewModel initialization completed.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while initializing StatisticViewModel.");
                MessageBox.Show("An error occurred while initializing statistics. Please try again later.");
            }
        }

        private void InitializeSeriesCollection()
        {
            _logger.Information("Initializing SeriesCollection.");
            SeriesCollection = new SeriesCollection();

            try
            {
                var salesData = GetSalesDataForLast30Days();
                AllDatesInLast30Days = GetDaysInLast30Days();
                XAxisLabels = AllDatesInLast30Days.Select(date => date.ToString("dd")).ToList();

                foreach (Order order in salesData)
                {
                    TotalMonth += order.Total;
                }

                var dayRevenuePairs = AllDatesInLast30Days
                  .Select((day, index) => new
                  {
                      Day = index,
                      Revenue = salesData.Where(order => order.Time.Date == day.Date).Sum(order => order.Total)
                  });

                var barSeries = new ColumnSeries
                {
                    Title = "Revenue: ",
                    Values = new ChartValues<ObservablePoint>(
                        dayRevenuePairs.Select(pair => new ObservablePoint(pair.Day, pair.Revenue))
                    )
                };

                SeriesCollection.Add(barSeries);
                _logger.Information("SeriesCollection initialized successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while initializing SeriesCollection.");
            }
        }

        private void InitializeSeriesPie()
        {
            _logger.Information("Initializing SeriesPie.");

            try
            {
                int bookSold = GetBooksSoldInCurrentMonth().Count();
                TotalBookMonth = bookSold;

                var (topBooks, titleCounts) = GetTopSellingBooksInCurrentMonth();

                if (bookSold == 0 || topBooks.Count < 3)
                {
                    _logger.Warning("Not enough data to calculate top sellers. Unable to illustrate Top sellers.");
                    MessageBox.Show("Not enough data to calculate. Unable to illustrate Top sellers!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int[] percentages = new int[3];
                double temp;
                for (int j = 0; j < 3; j++)
                {
                    temp = (double)titleCounts[j] / bookSold;
                    percentages[j] = (int)(100 * temp);
                }
                int others = 100 - percentages[0] - percentages[1] - percentages[2];

                Func<ChartPoint, string> labelPoint = ChartPoint =>
                string.Format(" {1:P}", ChartPoint.Y, ChartPoint.Participation);

                SeriesPie = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = topBooks[0].Title,
                        Values = new ChartValues<int>{percentages[0] },
                        DataLabels = true,
                        FontSize = 14,
                        LabelPoint = labelPoint
                    },
                    new PieSeries
                    {
                        Title = topBooks[1].Title,
                        Values = new ChartValues<int>{percentages[1] },
                        DataLabels = true,
                        FontSize = 14,
                        LabelPoint = labelPoint
                    },
                    new PieSeries
                    {
                        Title = topBooks[2].Title,
                        Values = new ChartValues<int>{percentages[2] },
                        DataLabels = true,
                        FontSize = 14,
                        LabelPoint = labelPoint
                    },
                    new PieSeries
                    {
                        Title = "Others",
                        Values = new ChartValues<int>{others },
                        DataLabels = true,
                        FontSize = 14,
                        LabelPoint = labelPoint
                    }
                };

                _logger.Information("SeriesPie initialized successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while initializing SeriesPie.");
            }
        }

        private List<Order> GetSalesDataForLast30Days()
        {
            _logger.Information("Fetching sales data for the last 30 days.");

            try
            {
                DataProvider<Order> db = new DataProvider<Order>(Order.Collection);
                DateTime startDate = DateTime.Now.Date.AddDays(-30);
                FilterDefinition<Order> filter = Builders<Order>.Filter.Gte(x => x.Time, startDate);
                List<Order> ordersInLast30Days = db.ReadFiltered(filter);

                _logger.Information("Fetched sales data for the last 30 days successfully.");
                return ordersInLast30Days;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while fetching sales data for the last 30 days.");
                return new List<Order>();
            }
        }

        private List<DateTime> GetDaysInLast30Days()
        {
            _logger.Information("Calculating the days in the last 30 days.");

            try
            {
                int daysInMonth = 30;
                DateTime today = DateTime.Now.Date;

                List<DateTime> daysInLast30Days = Enumerable.Range(0, daysInMonth)
                    .Select(day => today.AddDays(-day))
                    .Reverse()
                    .ToList();

                _logger.Information("Calculated days in the last 30 days successfully.");
                return daysInLast30Days;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while calculating the days in the last 30 days.");
                return new List<DateTime>();
            }
        }

        private List<DateTime> GetDaysInMonth(int year, int month)
        {
            _logger.Information("Calculating the days in the month {Month} of year {Year}.", month, year);

            try
            {
                int daysInMonth = DateTime.DaysInMonth(year, month);
                List<DateTime> daysInMonthList = Enumerable.Range(1, daysInMonth)
                    .Select(day => new DateTime(year, month, day))
                    .ToList();

                _logger.Information("Calculated the days in the month successfully.");
                return daysInMonthList;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while calculating the days in the month.");
                return new List<DateTime>();
            }
        }

        private List<Order> GetSalesDataForCurrentMonth()
        {
            _logger.Information("Fetching sales data for the current month.");

            try
            {
                DataProvider<Order> db = new DataProvider<Order>(Order.Collection);
                DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime startOfNextMonth = startOfMonth.AddMonths(1);

                FilterDefinition<Order> filter = Builders<Order>.Filter.And(
                    Builders<Order>.Filter.Gte(x => x.Time, startOfMonth),
                    Builders<Order>.Filter.Lt(x => x.Time, startOfNextMonth)
                );

                List<Order> ordersInCurrentMonth = db.ReadFiltered(filter);

                _logger.Information("Fetched sales data for the current month successfully.");
                return ordersInCurrentMonth;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while fetching sales data for the current month.");
                return new List<Order>();
            }
        }

        private List<Book> GetBooksSoldInCurrentMonth()
        {
            _logger.Information("Fetching books sold in the current month.");

            try
            {
                List<Order> currentSales = GetSalesDataForLast30Days();
                List<Book> booksSoldCurrent = new List<Book>();

                foreach (Order order in currentSales)
                {
                    foreach (OrderItem item in order.Items)
                    {
                        for (int k = 0; k < item.Quantity; k++)
                        {
                            if (!item.isGifted)
                                booksSoldCurrent.Add(Book.GetBook(item.BookID));
                        }
                    }
                }

                _logger.Information("Fetched books sold in the current month successfully.");
                return booksSoldCurrent;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while fetching books sold in the current month.");
                return new List<Book>();
            }
        }

        private (List<Book>, int[] titleCounts) GetTopSellingBooksInCurrentMonth()
        {
            _logger.Information("Fetching top-selling books in the current month.");

            try
            {
                List<Book> booksSold = GetBooksSoldInCurrentMonth();
                int[] titleCounts = new int[booksSold.Count];

                var groupedBooks = booksSold
                    .GroupBy(b => b.Title)
                    .Select(group => new
                    {
                        Title = group.First(),
                        Count = group.Count()
                    })
                    .OrderByDescending(group => group.Count)
                    .Take(3)
                    .ToList();

                var topTitles = groupedBooks.Select(group =>
                {
                    int index = Array.IndexOf(titleCounts, 0);
                    if (index != -1)
                    {
                        titleCounts[index] = group.Count;
                    }
                    return group.Title;
                }).ToList();

                _logger.Information("Fetched top-selling books in the current month successfully.");
                return (topTitles, titleCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while fetching top-selling books in the current month.");
                return (new List<Book>(), new int[0]);
            }
        }
    }
}

