using GroceryDeliverySystem.Models;
using GroceryDeliverySystem.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GroceryDeliverySystem.Controllers
{
    public class HomeController : Controller
    {
        groceryDBEntities gdb = new groceryDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View(gdb.Stores.ToList());
        }

        [HttpGet]
        [MyAuthorization]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [MyAuthorization]
        public ActionResult Login(Users u)
        {
            Users us = gdb.Users.FirstOrDefault(x => x.email == u.email && x.password == u.password);
            if (us != null)
            {
                FormsAuthentication.SetAuthCookie(u.email, false);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.hata = "Incorrect email or password!";
                return View();
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [MyAuthorization]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [MyAuthorization]
        public ActionResult Signup(Users u)
        {
            gdb.Users.Add(u);
            gdb.SaveChanges();
            var c = new Carts { userID = u.id };
            gdb.Carts.Add(c);
            gdb.SaveChanges();
            u.cartID = c.id;
            u.roles = "C";
            gdb.SaveChanges();
            if (u != null)
            {
                FormsAuthentication.SetAuthCookie(u.email, false);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.hata = "An error occurred!";
                return View();
            }
        }

        [HttpGet]
        [ChildActionOnly]
        [Authorize]
        public ActionResult NavBar1()
        {
            var userEmail = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(u => u.email == userEmail);
            var cart = gdb.Carts.FirstOrDefault(c => c.userID == user.id);
            var cartItems = gdb.CartItems.Where(x => x.cartID == cart.id).ToList();
            return PartialView("_NavBar1", cartItems);
        }

        [HttpGet]
        [ChildActionOnly]
        [Authorize]
        public ActionResult NavBar2()
        {
            var userEmail = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(u => u.email == userEmail);
            return PartialView("_NavBar2", user);
        }
    }
}