﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using WebShop.DAL;
using WebShop.Models;
using WebShop.ViewModels;

namespace WebShop.Controllers
{
    public class OrderController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        #region Model ViewModel Mapping
        private OrderViewModel ViewModelFromOrder(Order order)
        {
            var viewModel = new OrderViewModel()
            {
                OrderID = order.OrderID,
                ClientID = order.ClientID,
                OrderItems = order.OrderItems
            };
            return viewModel;
        }

        private void UpdateOrder(Order order, OrderViewModel viewModel)
        {
            order.OrderID = viewModel.OrderID;
            order.ClientID = viewModel.ClientID;
            order.OrderItems = viewModel.OrderItems;
        }
        #endregion

        #region DropDown lists helpers
        private SelectList PopulateClientList()
        {
            return new SelectList(unitOfWork.ClientRepository.Get(), "ID", "Name");
        }

        private SelectList PopulateProductList()
        {
            return new SelectList(unitOfWork.ProductRepository.Get(), "ID", "Name");
        }
        #endregion

        // GET: Order
        public ActionResult Index()
        {
            var orders = unitOfWork.OrderRepository.Get();
            return View(orders.ToList());
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            ViewBag.ClientID = PopulateClientList();
            ViewBag.ProductID = PopulateProductList();

            var order = unitOfWork.OrderRepository.CreateNewOrder();

            return View(ViewModelFromOrder(order));
        }

        [HttpPost]
        public ActionResult AddOrderItem(OrderViewModel viewModel)
        {
            viewModel.OrderItems.Add(unitOfWork.OrderRepository.CreateNewOrderItem());

            return Json(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteOrderItem(OrderViewModel viewModel, int index)
        {
            viewModel.OrderItems.RemoveAt(index);

            if (viewModel.OrderItems.Count == 0)
            {
                viewModel.OrderItems.Add(unitOfWork.OrderRepository.CreateNewOrderItem());
            }

            return Json(viewModel);
        }

        [HttpPost]
        public ActionResult Save(OrderViewModel viewModel)
        {
            if (viewModel.OrderID == 0)
            {
                // Insert new Order
                var order = unitOfWork.OrderRepository.CreateNewOrder();
                UpdateOrder(order, viewModel);

                unitOfWork.OrderRepository.Insert(order);
                unitOfWork.Save();
            }
            else 
            {
                Order order = unitOfWork.OrderRepository.GetByID(viewModel.OrderID);
                if (order == null)
                {
                    return HttpNotFound();
                }

                UpdateOrder(order, viewModel);
                unitOfWork.OrderRepository.Update(order);
                unitOfWork.Save();
            }

            //return Json(viewModel);
            return Redirect("~/");
        }

        // GET: Order/Edit/5
        [HttpGet]
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

            ViewBag.ClientID = PopulateClientList();
            ViewBag.ProductID = PopulateProductList();

            return View(ViewModelFromOrder(order));
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

        #region BASIC OPERATION
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
        #endregion

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
