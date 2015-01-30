using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using WebShop.Models;

namespace WebShop.DAL
{
    public class ShopInitializer : DropCreateDatabaseIfModelChanges<ShopContext>
    {
        protected override void Seed(ShopContext context)
        {
            var clients = new List<Client>
            {
                new Client{Name = "Carson", Address="USA, California", Telephone = "+12000999"},
                new Client{Name = "John", Address="RF, Moscow", Telephone = "+79666666"},
                new Client{Name = "Vasily", Address="UA, Lviv", Telephone = "+3804567890"}                
            };

            clients.ForEach(s => context.Clients.Add(s));
            context.SaveChanges();

            var products = new List<Product>
            {
                new Product{Name = "Soap", Weight=250.0, Stock = 100},
                new Product{Name = "Apples", Weight=890.0, Stock = 5},
                new Product{Name = "Sand", Weight=10.0, Stock = 200}            
            };

            products.ForEach(s => context.Products.Add(s));
            context.SaveChanges();
        }
    }
}