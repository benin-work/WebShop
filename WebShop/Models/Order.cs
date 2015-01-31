using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        
        [Required]
        public int ClientID { get; set; }

        public virtual Client Client { get; set; }
        public virtual IList<OrderItem> OrderItems { get; set; }
    }
}