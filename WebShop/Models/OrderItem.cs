using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }
        
        [Required]
        public int OrderID { get; set; }
        
        [Required]
        public int ProductID { get; set; }
        
        [Required]
        public int Qty { get; set; }

        public virtual Product Product { get; set; }
    }
}