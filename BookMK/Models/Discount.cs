using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
    public class Discount
    {
        private string ID { get; set; }
        public float DiscountAmount { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; }
    }
}
