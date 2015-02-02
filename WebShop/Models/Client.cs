using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WebShop.Models
{
    public class Client
    {
        [Key]
        public int ID { get; set; }
       
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
        
        public string Telephone { get; set; }
       
        public string Address { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}