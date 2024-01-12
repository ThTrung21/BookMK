using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]

        public int ID { get; set; }
        public ObservableCollection<OrderItem> Items { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        public int StaffID { get; set; }
        public string StaffName { get; set; }

       

        public double Total { get; set; }
        public DateTime Time { get; set; }

        public static string Collection = "orders";



        public static int CreateID()
        {
            DataProvider<Order> db = new DataProvider<Order>(Order.Collection);
            List<Order> allcs = db.ReadAll().OrderBy(p => p.ID).ToList();
            int expectedValue = 0;
            foreach (var o in allcs)
            {
                int num = o.ID;
                if (num == expectedValue)
                {
                    // Increment the expected value if it matches the current number
                    expectedValue++;
                }
                else
                {
                    // Found the smallest missing integer
                    return expectedValue;
                }
            }
            return expectedValue;
        }
    }
}
