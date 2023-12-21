using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]

        protected int ID { get; set; }

        //avatar
        public string Cover { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }

        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        
        public float SellPrice { get; set; }
        public int Stock { get; set; }

        public Book() { }


    }
}
