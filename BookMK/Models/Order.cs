using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
    public class Order
    {
        protected int ID { get; set; }
        public int CustomerID { get; set; }
        
        public DateTime BillDate { get; set; }

        public int DIscountID { get; set; }
        public float DiscountAmount { get; set; }

        public int Total { get; set; }

        public List<OrderItem> OrderList { get; set; }
    }
}
