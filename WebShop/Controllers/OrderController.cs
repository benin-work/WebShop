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
            ViewBag.ClientID = new SelectList(unitOfWork.ClientRepository.Get(), "ID", "Name");
            ViewBag.ProductID = new SelectList(unitOfWork.ProductRepository.Get(), "ID", "Name");

            var order = unitOfWork.OrderRepository.CreateNewOrder();

            return View(order);
        }

        public ActionResult AddOrderItem(Order order)
        {
            order.OrderItems.Add(unitOfWork.OrderRepository.CreateNewOrderItem());

            return Json(order);
        }
        public ActionResult DeleteOrderItem(Order order, int index)
        {
            order.OrderItems.RemoveAt(index);

            if (order.OrderItems.Count == 0)
            {
                order.OrderItems.Add(unitOfWork.OrderRepository.CreateNewOrderItem());
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
                unitOfWork.OrderRepository.Insert(order);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(unitOfWork.ClientRepository.Get(), "ID", "Name", order.ClientID);
            return View(order);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ClientID = new SelectList(unitOfWork.ClientRepository.Get(), "ID", "Name", order.ClientID);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,ClientID")] Order order)
        {
            return new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);
            //if (ModelState.IsValid)
            //{
            //    db.Entry(order).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.ClientID = new SelectList(db.Clients, "ID", "Name", order.ClientID);
            //return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = unitOfWork.OrderRepository.GetByID(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            unitOfWork.OrderRepository.Delete(order);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
