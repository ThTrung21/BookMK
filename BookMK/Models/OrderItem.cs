using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
    public class OrderItem
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]

        public ObjectId Id { get; set; }
        //get from Book
        public string SellBook { get; set; }
        public int BookID { get; set; }

        public bool isGifted { get; set; }

        
        


        public int Quantity { get; set; }
        public double ItemPrice { get; set; }
        
        public void AddOne(string b, int id, double price, int a)
        {

            this.SellBook = b;
            this.BookID = id;
            
            this.Quantity = a;
            this.ItemPrice = a * price;
        }

    }
}
