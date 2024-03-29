﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace BookMK.Models
{
    public class Staff
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]
        public int ID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        //admin=3 staff=2
        public string Role { get; set; }


        public static string Collection = "staffs";



        public static int CreateID()
        {
            DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
            List<Staff> allcs = db.ReadAll().OrderBy(p => p.ID).ToList();
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


            DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);
            FilterDefinition<Staff> filter = Builders<Staff>.Filter.Regex(x => x.Username, new BsonRegularExpression("^" + Regex.Escape(Name) + "$", "i"));

            //List<Staff> resultls = db.ReadFiltered(filter);
            //bool exists = resultls.Count > 0;

            //return exists;
            return db.collection.Find(filter).Any();


        }
       
        public static bool IsExisted(string Name, string Phone)
        {
            DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);

            // Use a filter that checks both Username and Phone
            FilterDefinition<Staff> filter = Builders<Staff>.Filter.And(
                Builders<Staff>.Filter.Regex(x => x.Username, new BsonRegularExpression("^" + Regex.Escape(Name) + "$", "i")),
                Builders<Staff>.Filter.Regex(x => x.Phone, new BsonRegularExpression("^" + Regex.Escape(Phone) + "$", "i"))
            );

            return db.collection.Find(filter).Any();
        }



        //hash password before storing in database
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

      public static Staff IsStaffEmailExist(string email)
        {
            DataProvider<Staff> db = new DataProvider<Staff>(Staff.Collection);

            // Use a filter that checks both Username and Phone
            FilterDefinition<Staff> filter = Builders<Staff>.Filter.And(
                 Builders<Staff>.Filter.Eq(x => x.Email, email),
                  Builders<Staff>.Filter.Eq(x => x.IsVerified, true));

            return db.collection.Find(filter).FirstOrDefault();
        }


       
    }
   


}
