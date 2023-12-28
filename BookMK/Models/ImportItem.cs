using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
    public class ImportItem
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; }
        //get the info of the book item
        public Book ImportBook { get; set; }
        //per one book
        public double UnitPrice { get; set; }

        public int Amount { get; set; }
        public double  ItemPrice{ get; set; }
    }
}
