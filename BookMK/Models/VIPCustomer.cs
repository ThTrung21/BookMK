using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMK.Models
{
    internal class VIPCustomer: Customer
    {
        protected int CustomerID { get; }  // Reference to Customer
        public float Debt { get; set; }
    }
}
