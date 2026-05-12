using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcommerceWeb.Models;

namespace EcommerceWeb.Controllers
{
    public class OrdersController : Controller
    {
        private ECommerceDBEntities db = new ECommerceDBEntities();

        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Agent);
            return View(orders.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id.ToString());
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");
            return View(order);
        }

        public ActionResult Create()
        {
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,OrderDate,AgentID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = order.OrderID });
            }

            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", order.AgentID);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDetail(string maDon, string maSp, int Quantity)
        {
            var item = db.Items.Find(maSp);

            if (item != null)
            {
                OrderDetail detail = new OrderDetail();
                detail.OrderID = maDon;
                detail.ItemID = maSp;
                detail.Quantity = Quantity;
                detail.UnitAmount = item.Price;

                db.OrderDetails.Add(detail);
                db.SaveChanges();
            }

            int idConvert = 0;
            int.TryParse(maDon, out idConvert);

            return RedirectToAction("Details", new { id = idConvert });
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Order order = db.Orders.Find(id.ToString());
            if (order == null) return HttpNotFound();

            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", order.AgentID);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,OrderDate,AgentID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", order.AgentID);
            return View(order);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id.ToString());
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

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
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}