using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebShop.DAL;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class OrderController : Controller
    {
        private ShopContext db = new ShopContext();
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Order
        public ActionResult Index()
        {
            var orders = unitOfWork.OrderRepository.Get();
            return View(orders.ToList());
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = unitOfWork.OrderRepository.GetByID(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name");
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");

            var order = unitOfWork.OrderRepository.CreateNewOrder();

            return View(order);
        }

        public ActionResult AddOrderItem(Order order)
        {
            var products = unitOfWork.ProductRepository.Get();
            var orderItem = new OrderItem { OrderID = order.OrderID, ProductID = products.FirstOrDefault().ID};
            order.OrderItems.Add(orderItem);

            return Json(order);
        }
        public ActionResult DeleteOrderItem(Order order, int index)
        {
            order.OrderItems.RemoveAt(index);

            if (order.OrderItems.Count == 0)
            {
                var products = unitOfWork.ProductRepository.Get();
                var orderItem = new OrderItem { OrderID = order.OrderID, ProductID = products.FirstOrDefault().ID };
                order.OrderItems.Add(orderItem);
            }
            
            return Json(order);
        }
        

        public ActionResult Save(Order order)
        {
            unitOfWork.OrderRepository.Insert(order);
            unitOfWork.Save();

            return Json(order);
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,ClientID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name", order.ClientID);
            return View(order);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name", order.ClientID);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,ClientID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name", order.ClientID);
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
