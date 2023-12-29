using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookMK.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]

        public int ID { get; set; }

        //avatar
        public string Cover { get; set; }
        public string Title { get; set; }

        //should this be a stirng list
        public List<string> Genre { get; set; }
        public string ReleaseYear { get; set; }

        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        
        public double SellPrice { get; set; }
        public int Stock { get; set; }
        public static string Collection = "books";

        public Book() { }

        public static int CreateID()
        {
            DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
            List<Book> allcs = db.ReadAll().OrderBy(p => p.ID).ToList();
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

        public static bool IsExisted(string Name, string Author, string Year)
        {


            DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
            FilterDefinition<Book> filter = Builders<Book>.Filter.And(
                Builders<Book>.Filter.Regex(x => x.Title, new BsonRegularExpression("^" + Regex.Escape(Name) + "$", "i")),
                Builders<Book>.Filter.Regex(x => x.AuthorName, new BsonRegularExpression("^" + Regex.Escape(Author) + "$", "i")),
                Builders<Book>.Filter.Regex(x => x.ReleaseYear, new BsonRegularExpression("^" + Regex.Escape(Year) + "$", "i"))
                
                );

            //List<Staff> resultls = db.ReadFiltered(filter);
            //bool exists = resultls.Count > 0;

            //return exists;
            return db.collection.Find(filter).Any();


        }
    }
}
