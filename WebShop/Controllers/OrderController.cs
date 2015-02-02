using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using AutoMapper;

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
            return Mapper.Map<Order, OrderViewModel>(order);
        }

        private void UpdateOrderFromViewModel(OrderViewModel viewModel, Order order)
        {
            Mapper.CreateMap<OrderItemViewModel, OrderItem>()
            .ConstructUsing((OrderItemViewModel itemVM) =>
            {
                if (itemVM.OrderItemID == 0)
                {
                    return unitOfWork.OrderRepository.CreateNewOrderItem();
                }
                return unitOfWork.OrderItemRepository.GetByID(itemVM.OrderItemID);
            });

            Mapper.CreateMap<OrderViewModel, Order>()
            .ConstructUsing((OrderViewModel orderVM) =>
            {
                if (orderVM.OrderID == 0)
                {
                    return unitOfWork.OrderRepository.CreateNewOrder();
                }
                return unitOfWork.OrderRepository.GetByID(orderVM.OrderID);
            });

            // It works only 
            //find out what details no longer exist in the 
            //DTO and delete the corresponding entities 
            //.BeforeMap((dto, o) =>
            //{
            //    o
            //    .OrderItems
            //    .Where(d => !dto.OrderItems.Any(ddto => ddto.OrderItemID == d.OrderItemID))
            //    .ToList()
            //    .ForEach(deleted =>
            //        {
            //            unitOfWork.OrderItemRepository.Delete(deleted);
            //        });
            //});

            // Find out what items no longer exist in the model and delete them
            order.OrderItems.Where(d =>
                !viewModel.OrderItems.Any(o => o.OrderItemID == d.OrderItemID))
                .ToList()
                .ForEach(deleted => unitOfWork.OrderItemRepository.Delete(deleted));

            Mapper.Map(viewModel, order);
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
            var item = unitOfWork.OrderRepository.CreateNewOrderItem();
            OrderItemViewModel itemVM = Mapper.Map<OrderItem, OrderItemViewModel>(item);
            viewModel.OrderItems.Add(itemVM);

            return Json(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteOrderItem(OrderViewModel viewModel, int index)
        {
            viewModel.OrderItems.RemoveAt(index);

            if (viewModel.OrderItems.Count == 0)
            {
                var item = unitOfWork.OrderRepository.CreateNewOrderItem();
                OrderItemViewModel itemVM = Mapper.Map<OrderItem, OrderItemViewModel>(item);
                viewModel.OrderItems.Add(itemVM);
            }

            return Json(viewModel);
        }

        [HttpPost]
        public ActionResult Save(OrderViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Order details error!");

            // Check Stock
            bool valid = true;
            IDictionary<int, int> products = new Dictionary<int, int>();
            viewModel.OrderItems.ToList().ForEach(i => 
            {
                if (products.ContainsKey(i.ProductID))
                {
                    products[i.ProductID] += i.Qty;
                }
                else 
                {
                    products[i.ProductID] = i.Qty;
                }
            });

            string productName ="";
            int productStock = 0;
            foreach (var item in products)
            {
                var product = unitOfWork.ProductRepository.GetByID(item.Key);
                if (item.Value > product.Stock)
                {
                    productName = product.Name;
                    productStock = product.Stock;
                    valid = false;
                    break;
                }
            }

            if (!valid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, String.Format("Product {0} out of stock ({1})!", productName, productStock));
            
            // Save/Update data
            if (viewModel.OrderID == 0)
            {
                // Insert new Order
                var order = unitOfWork.OrderRepository.CreateNewOrder();
                UpdateOrderFromViewModel(viewModel, order);

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

                //UpdateOrder(order, viewModel);
                UpdateOrderFromViewModel(viewModel, order);
                unitOfWork.OrderRepository.Update(order);
                unitOfWork.Save();
            }

            return Json(viewModel);
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

        [HttpGet]
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
