using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebShop.ViewModels
{
    public class OrderItemViewModel
    {
        public int OrderItemID { get; set; }
        
        [Required]
        public int OrderID { get; set; }
        
        [Required]
        public int ProductID { get; set; }
        
        [Required]
        public int Qty { get; set; }

     }
}