using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace BookMK.Models
{
    public class Customer: Account
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]

        public int ID { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int CitizenID { get; set; }
       
        public int PurchasePoint { get; set; }
       

        public static string Collection = "customer";


        public static int CreateID()
        {
            DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
            List<Customer> allcs = db.ReadAll().OrderBy(p => p.ID).ToList();
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
        public static bool IsExisted(string Name, string Phone)
        {
            DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
            FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(x => x.Fullname, Name)
                & Builders<Customer>.Filter.Eq(x => x.Phone, Phone);
            List<Customer> resultls = db.ReadFiltered(filter);
            return resultls.Count > 0;
        }
        public override string ToString()
        {
            if (this.Fullname != null)
                return this.Fullname;
            return "";
        }
        public static Customer GetCustomerByName(string Name)
        {
            if (Name == null)
            {
                return null;
            }
            Customer c = new Customer();
            DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
            FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(x => x.Fullname, Name);
            List<Customer> customers = db.ReadFiltered(filter);
            if (customers.Count > 0)
            {
                return customers[0];
            }
            else
                return null;
        }
    }
}
