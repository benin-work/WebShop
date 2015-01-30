using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public int Stock { get; set; }
    }
}