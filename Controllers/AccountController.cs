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
            ViewBag.Cities = gdb.Cities.Where(x => x.isActive == true).ToList();
            var email = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(x => x.email == email);
            return View(user);
        }

        public ActionResult Orders()
        {
            var email = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(x => x.email == email);
            var ord = gdb.Orders.Where(x => x.userID == user.id && x.isActive == true).ToList();
            return View(ord);
        }

        public ActionResult GetOrderItems(int orderID)
        {
            var orderItems = gdb.OrderItems.Where(o => o.orderID == orderID).ToList();
            var order = gdb.Orders.FirstOrDefault(o => o.id == orderID);
            ViewBag.Order = order;
            ViewBag.CityName = gdb.Cities.FirstOrDefault(x => x.id == order.cityID).name;
            return PartialView("_OrderItemsPartial", orderItems);
        }

        public ActionResult UpdateUser(Users user)
        {
            bool isAvailable = gdb.Users.Any(u => u.email == user.email && u.id != user.id);
            if (isAvailable)
            {
                TempData["EmailError"] = "The email address you've entered is already in use. Please choose another one.";
            }
            else
            {
                gdb.Users.AddOrUpdate(user);
                gdb.SaveChanges();
            }
            return RedirectToAction("Settings");
        }

        public ActionResult UpdateAddress(int userID, string address, int cityID)
        {
            var user = gdb.Users.FirstOrDefault(x => x.id == userID);
            user.address = address;
            if (user.cityID != cityID)
            {
                var cartItems = gdb.CartItems.Where(x => x.cartID == user.cartID).ToList();
                gdb.CartItems.RemoveRange(cartItems);
            }
            user.cityID = cityID;
            gdb.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}