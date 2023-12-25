using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace BookMK.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]

        public int ID { get; set; }
        public string Role {get;set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int PurchasePoint { get; set; }
        public bool IsVerified { get; set; }

        public static string Collection = "customers";


        public static int CreateID()
        {
            DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
            List<Customer> allcs = db.ReadAll().OrderBy(p => p.ID).ToList();
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
        public static bool IsExisted(string Name)
        {
            DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);
            FilterDefinition<Customer> filter = Builders<Customer>.Filter.Regex(x => x.Username, new BsonRegularExpression("^" + Regex.Escape(Name) + "$", "i"));

            //List<Customer> resultls = db.ReadFiltered(filter);
            //bool exists = resultls.Count > 0;

            //return exists;
            return db.collection.Find(filter).Any();


        }
        public static bool IsExisted(string Name, string Phone)
        {
            DataProvider<Customer> db = new DataProvider<Customer>(Customer.Collection);

            // Use a filter that checks both Username and Phone
            FilterDefinition<Customer> filter = Builders<Customer>.Filter.And(
                Builders<Customer>.Filter.Regex(x => x.Username, new BsonRegularExpression("^" + Regex.Escape(Name) + "$", "i")),
                Builders<Customer>.Filter.Regex(x => x.Phone, new BsonRegularExpression("^" + Regex.Escape(Phone) + "$", "i"))
            );

            return db.collection.Find(filter).Any();
        }


        public override string ToString()
        {
            if (this.FullName != null)
                return this.FullName;
            return "";
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }


        
    }
}
