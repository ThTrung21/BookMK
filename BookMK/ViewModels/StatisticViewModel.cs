using BookMK.Models;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Points;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private SeriesCollection _seriespie;
        public SeriesCollection SeriesPie
        {
            get { return _seriespie; }
            set
            {
                _seriespie = value;
                OnPropertyChanged(nameof(SeriesPie));
            }
        }

        public List<DateTime> AllDatesInLast30Days { get; set; }

        public int TotalBookMonth { get;set; }

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
            DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
            List<Book> a =  Book.GetOutOfStock();

            this.OutOfStockBooks = new ObservableCollection<Book>(a);

            //=====================================================================
            //current month revenue
            //SeriesCollection = new SeriesCollection();
            //var salesData = GetSalesDataForCurrentMonth();
            //AllDatesInMonth = GetDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            //foreach ( Order o in salesData)
            //{
            //    TotalMonth += o.Total;
            //}

            //var dayRevenuePairs = AllDatesInMonth
            //.Select(day => new
            //{
            //    Day = day.Day,
            //    Revenue = salesData.Where(order => order.Time.Day == day.Day).Sum(order => order.Total)
            //});
            //var barSeries = new ColumnSeries
            //{
            //    Title = "Revenue: ",
            //    Values = new ChartValues<ObservablePoint>(
            //        dayRevenuePairs.Select(pair => new ObservablePoint(pair.Day, pair.Revenue))
            //    )
            //};

            //// Add the series to the chart
            //SeriesCollection.Add(barSeries);
            SeriesCollection = new SeriesCollection();
            

            var salesData = GetSalesDataForLast30Days();
           // AllDatesInMonth = GetDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            AllDatesInLast30Days = GetDaysInLast30Days();
            XAxisLabels = AllDatesInLast30Days.Select(date => date.ToString("dd")).ToList();
            foreach (Order o in salesData)
            {
                TotalMonth += o.Total;
            }

            var dayRevenuePairs = AllDatesInLast30Days
              .Select((day, index) => new
              {
                  Day = index ,
                  Revenue = salesData.Where(order => order.Time.Date == day.Date).Sum(order => order.Total)
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


            //========================================================================
            //Top sellers current month
            #region Pie


            int BookSold = GetBooksSoldInCurrentMonth().Count();
            TotalBookMonth= GetBooksSoldInCurrentMonth().Count();


            var (topBooks, titleCounts)=GetTopSellingBooksInCurrentMonth();
            if (BookSold == 0 ||topBooks.Count<3)
            {
                MessageBox.Show("Not enough data to calculate. Unable to illustrate Top sellers!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            int[] percentages= new int[3];
            double temp;
            for(int j=0;j<3;j++)
            {
                temp =(double) titleCounts[j]/BookSold;
                percentages[j] = (int)( 100*temp) ;
            }
            int others = 100 - percentages[0] - percentages[1] - percentages[2];

            

            Func<ChartPoint, string> labelpoint = ChartPoint =>
            string.Format(" {1:P}", ChartPoint.Y, ChartPoint.Participation);
            SeriesPie = new SeriesCollection
            {
                new PieSeries
                {
                    Title=topBooks[0].Title,
                    Values = new ChartValues<int>{percentages[0] },
                    DataLabels=true,
                    FontSize=14,
                    
                    LabelPoint=labelpoint
                },
                new PieSeries
                {
                    Title=topBooks[1].Title,
                    Values = new ChartValues<int>{percentages[1] },
                    DataLabels=true,
                     FontSize=14,
                    LabelPoint=labelpoint
                },
                new PieSeries
                {
                    Title=topBooks[2].Title,
                    Values = new ChartValues<int>{percentages[2] },
                    DataLabels=true,
                     FontSize=14,
                    LabelPoint=labelpoint
                },
                new PieSeries
                {
                    Title="Others",
                    Values = new ChartValues<int>{others },
                    DataLabels=true,
                     FontSize=14,
                    LabelPoint=labelpoint
                }

            };



            #endregion
        }

        private List<Order> GetSalesDataForLast30Days()
        {
            DataProvider<Order> db = new DataProvider<Order>(Order.Collection);

            // Calculate the start date as today minus 30 days
            DateTime startDate = DateTime.Now.Date.AddDays(-30);

            // Filter orders within the last 30 days
            FilterDefinition<Order> filter = Builders<Order>.Filter.Gte(x => x.Time, startDate);

            List<Order> ordersInLast30Days = db.ReadFiltered(filter);

            return ordersInLast30Days;
        }
        private List<DateTime> GetDaysInLast30Days()
        {
            int daysInMonth = 30; // Assuming you want the last 30 days
            DateTime today = DateTime.Now.Date;

            return Enumerable.Range(0, daysInMonth)
                .Select(day => today.AddDays(-day))
                .Reverse() // Reverse the order to display from right to left
                .ToList();
        }



        //old function for current month
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




        //get top books
        private List<Book> GetBooksSoldInCurrentMonth()
        {
            List<Order> currentSales = GetSalesDataForLast30Days();
            List<Book> BookSoldCurrent = new List<Book>();
            foreach (Order o in currentSales)
            {
                foreach(OrderItem i in o.Items)
                {
                    for(int k=0;k<i.Quantity;k++)
                    {
                        if (!i.isGifted)
                            BookSoldCurrent.Add(Book.GetBook(i.BookID));
                    }
                }    
            }
            return BookSoldCurrent;
        }
        private (List<Book>, int[] titleCounts) GetTopSellingBooksInCurrentMonth()
        {
           List<Book> a = GetBooksSoldInCurrentMonth();
            int[] titleCounts=new int[a.Count];
            
            var groupedBooks = a
                .GroupBy(b => b.Title)
                .Select(group => new
                {
                    Title = group.First(),
                    Countc = group.Count()
                })
                .OrderByDescending(group => group.Countc)
                .Take(3)
                .ToList();
                
            var TopTitles= groupedBooks.Select(group =>
            {
                int index = Array.IndexOf(titleCounts, 0);
                if(index!=-1)
                {
                    titleCounts[index] = group.Countc;
                }
                return group.Title;
            }).ToList();

           return (TopTitles,titleCounts);
        }
    }
   
}
