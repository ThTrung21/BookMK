using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BookMK.ViewModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace BookMK.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]



        public ObjectId _id { get; set; }

        //admin=3   staff=2 customer=1
        public int Role { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }



        public Account() { }

        public static string Collection = "account";





        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        //this method to find the acocunt by phone number (this is used as username to login with an exception of admin user's username being 'admin')
        public static Account GetAccountByUsername(string phone)
        {
            if (phone == null)
            {
                return null;
            }
            Account c = new Account();
            DataProvider<Account> db = new DataProvider<Account>(Account.Collection);
            FilterDefinition<Account> filter = Builders<Account>.Filter.Eq(x => x.Username, phone);
            List<Account> accounts = db.ReadFiltered(filter);
           
            return accounts.FirstOrDefault();
        }
    }
}
