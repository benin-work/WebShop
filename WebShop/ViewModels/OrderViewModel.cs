using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebShop.Models;
using WebShop.DAL;

namespace WebShop.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        public int OrderID { get; set; }
        
        [Required]
        public int ClientID { get; set; }

        public Client Client { get; set; }
        public IList<OrderItem> OrderItems { get; set; }

        public IList<SelectListItem> ClientDropDown { get; set; }
    }
}