using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WebShop.Models;

namespace WebShop.DAL
{
    public class UnitOfWork: IDisposable
    {
        private ShopContext context = new ShopContext();
        private GenericShopRepository<Client> clientRepository;
        private GenericShopRepository<Product> productRepository;
        private GenericShopRepository<OrderItem> orderItemsRepository;
        private OrderRepository orderRepository;

        public GenericShopRepository<Client> ClientRepository
        {
            get
            {
                if (this.clientRepository == null)
                {
                    this.clientRepository = new GenericShopRepository<Client>(context);
                }
                return clientRepository;
            }
        }

        public GenericShopRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new GenericShopRepository<Product>(context);
                }
                return productRepository;
            }
        }

        public GenericShopRepository<OrderItem> OrderItemRepository
        {
            get
            {
                if (this.orderItemsRepository == null)
                {
                    this.orderItemsRepository = new GenericShopRepository<OrderItem>(context);
                }
                return orderItemsRepository;
            }
        }
        

        public OrderRepository OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository = new OrderRepository(context);
                }
                return orderRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}