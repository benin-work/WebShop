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
    public class ClientController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Client
        public ActionResult Index()
        {
            return View(unitOfWork.ClientRepository.Get());
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Telephone,Address")] Client client)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ClientRepository.Insert(client);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = unitOfWork.ClientRepository.GetByID(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Telephone,Address")] Client client)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ClientRepository.Update(client);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Client/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = unitOfWork.ClientRepository.GetByID(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = unitOfWork.ClientRepository.GetByID(id);
            unitOfWork.ClientRepository.Delete(client);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
