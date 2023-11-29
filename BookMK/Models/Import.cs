using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
    public class Import
    {
        protected int BookID { get; }
        public int ImportAmount { get; set; }
        public double Price { get; set; }
        public DateTime Time { get; set; }

       

        public Import()
        {
            //check if stock is <300 then add to stock
        }
    }
}
