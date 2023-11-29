using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
    public class OrderItem
    {
        public string ID { get; }
        //get from Book
        public int BookID { get; }
        public string Title { get; }
        public int Price { get; }


        public int Quantity { get; set; }
    
        
    }
}
