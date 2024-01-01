using BookMK.Commands.InsertCommand;
using MongoDB.Bson;
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
        
        public ObjectId Id { get; set; }
        public string ImportBook { get; set; }
        public int BookID { get; set; }
        //per one book
        public double UnitPrice { get; set; }

        public int Amount { get; set; }
        public double  ItemPrice{ get; set; }



        public void AddOne(string b,int id ,double price, int a)
        {

            this.ImportBook = b;
            this.BookID = id;
            this.UnitPrice = price;
            this.Amount = a;
            this.ItemPrice = a * price;
        }
    }
}
