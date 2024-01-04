using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
    public class Discount
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]
        public int ID { get; set; }
        
        public string Type { get; set; }
        
        public int BookID { get; set; }
        public int BookID_free { get; set; }

        
      
        public DateTime Time { get; set; }

        public int EligibleBill { get; set; }
        public int Value { get; set; }
        public static string Collection = "discounts";


        public static int CreateID()
        {
            DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
            List<Discount> allcs = db.ReadAll().OrderBy(p => p.ID).ToList();
            int expectedValue = 1;
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
