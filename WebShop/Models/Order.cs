using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int ClientID { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}