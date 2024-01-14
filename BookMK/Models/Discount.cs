using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public int PointMileStone { get;set; }
        public int Value { get; set; }
        public static string Collection = "discounts";


        public static int CreateID()
        {
            DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
            List<Discount> allcs = db.ReadAll().OrderBy(p => p.ID).ToList();
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
        


        //insert command check
        public static bool IsExistedPercentageAll()
        {
            DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);

            // Use a filter that checks both Username and Phone
            FilterDefinition<Discount> filter = Builders<Discount>.Filter.And(
               Builders<Discount>.Filter.Eq(x => x.BookID, 0),
               Builders<Discount>.Filter.Eq(x => x.Type,"Percentage" )
           );
            return db.collection.Find(filter).Any();
        }
        public static bool IsExistedLoyal()
        {
            DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);

            // Use a filter that checks both Username and Phone
            FilterDefinition<Discount> filter = Builders<Discount>.Filter.And(
               Builders<Discount>.Filter.Eq(x => x.ID, 0),
               Builders<Discount>.Filter.Eq(x => x.Type, "Loyal")
           );
            return db.collection.Find(filter).Any();
        }

        public override string ToString()
        {
            if (this.Value != 0)
                return this.Value.ToString();
            return "";
        }
        public static List<Discount> GetDiscountAmountList()
        {
            DataProvider<Discount> db = new DataProvider<Discount>(Discount.Collection);
            FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq(x => x.Type, "Amount");


            List<Discount> eligibleDiscounts = db.ReadFiltered(filter);
            return eligibleDiscounts;
        }
    }
}
