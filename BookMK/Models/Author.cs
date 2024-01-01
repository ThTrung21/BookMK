using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
    public class Author
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]

        public int ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public static string Collection = "author";


        //Create a user firendly ID
        public static int CreateID()
        {
            DataProvider<Author> db = new DataProvider<Author>(Author.Collection);
            List<Author> authorlist = db.ReadAll().OrderBy(p => p.ID).ToList();
            int expectedValue = 1;
            foreach (var o in authorlist)
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

        //check duplicate when added
        public static bool IsExisted(string Name)
        {
            DataProvider<Author> db = new DataProvider<Author>(Author.Collection);

            FilterDefinition<Author> filter = Builders<Author>.Filter.Eq(x => x.Name, Name)
               ;
            List<Author> resultls = db.ReadFiltered(filter);
            return resultls.Count > 0;
        }


        public override string ToString()
        {
            if (this.Name != null)
                return this.Name;
            return "";
        }

        //search author
        public static Author GetAuthorByName(string Name)
        {
            if (Name == null)
            {
                return null;
            }

            Author c = new Author();
            DataProvider<Author> db = new DataProvider<Author>(Author.Collection);

            FilterDefinition<Author> filter = Builders<Author>.Filter.Eq(x => x.Name, Name);
            List<Author> authors = db.ReadFiltered(filter);
            if (authors.Count > 0)
            {
                return authors[0];
            }
            else
                return null;
        }

        public static List<Author> GetAuthorsList()
        {
            DataProvider<Author> db = new DataProvider<Author>(Author.Collection);
            List<Author> AllAuthors = db.ReadAll();

            return AllAuthors;
        }


       
    }
    

}
