using GroceryDeliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;

namespace GroceryDeliverySystem.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        groceryDBEntities gdb = new groceryDBEntities();
        // GET: Account
        public ActionResult Settings()
        {
            ViewBag.Cities = gdb.Cities.ToList();
            var email = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(x => x.email == email);
            return View(user);
        }

        public ActionResult Orders()
        {
            var email = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(x => x.email == email);
            var ord = gdb.Orders.Where(x => x.userID == user.id).ToList();
            return View(ord);
        }

        public ActionResult GetOrderItems(int orderId)
        {
            var orderItems = gdb.OrderItems.Where(o => o.orderID == orderId).ToList();
            return PartialView("_OrderItemsPartial", orderItems);
        }

        public ActionResult UpdateUser(Users user)
        {
            gdb.Users.AddOrUpdate(user);
            gdb.SaveChanges();
            return RedirectToAction("Settings");
        }
    }
}