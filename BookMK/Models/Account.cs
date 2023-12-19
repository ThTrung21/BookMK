using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMK.ViewModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace BookMK.Models
{
    public class Account: ViewModelBase
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]
       
        
        public string Password{get;set;}

        private string _email = "";
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public Account() { }
        public static string Collection = "account";
    }
}
