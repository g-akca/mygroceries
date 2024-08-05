using GroceryDeliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace GroceryDeliverySystem.Controllers
{
    [Authorize(Roles = "A, S")]
    public class PanelController : Controller
    {
        groceryDBEntities gdb = new groceryDBEntities();
        // GET: Panel

        public ActionResult Error()
        {
            return View();
        }

        [Authorize(Roles = "A")]
        public ActionResult Users()
        {
            ViewBag.Cities = gdb.Cities.Where(x => x.isActive == 0).ToList();
            ViewBag.Stores = gdb.Stores.Where(x => x.isActive == 0).ToList();

            var userEmail = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(x => x.email == userEmail);
            if (User.IsInRole("A") || user.managedStore != null)
            {
                return View(gdb.Users.Where(x => x.isActive == 0).ToList());
            }
            else
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "A")]
        public ActionResult Cities()
        {
            var userEmail = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(x => x.email == userEmail);
            if (User.IsInRole("A") || user.managedStore != null)
            {
                return View(gdb.Cities.Where(x => x.isActive == 0).ToList());
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Stores()
        {
            ViewBag.Cities = gdb.Cities.Where(x => x.isActive == 0).ToList();
            if (User.IsInRole("A"))
            {
                return View(gdb.Stores.Where(x => x.isActive == 0).ToList());
            }
            else
            {
                var userEmail = User.Identity.Name;
                var user = gdb.Users.FirstOrDefault(x => x.email == userEmail);
                if (user.managedStore != null)
                {
                    return View(gdb.Stores.Where(x => x.id == user.managedStore && x.isActive == 0).ToList());
                }
                else
                {
                    return View("Error");
                }
            }
        }

        public ActionResult Categories()
        {
            ViewBag.Stores = gdb.Stores.Where(x => x.isActive == 0).ToList();
            if (User.IsInRole("A"))
            {
                return View(gdb.Categories.Where(x => x.isActive == 0).ToList());
            }
            else
            {
                var userEmail = User.Identity.Name;
                var user = gdb.Users.FirstOrDefault(x => x.email == userEmail);
                if (user.managedStore != null)
                {
                    var store = gdb.Stores.FirstOrDefault(x => x.id == user.managedStore);
                    return View(gdb.Categories.Where(x => x.storeID == store.id && x.isActive == 0).ToList());
                }
                else
                {
                    return View("Error");
                }
            }
        }

        public ActionResult Products()
        {
            if (User.IsInRole("A"))
            {
                ViewBag.Categories = gdb.Categories.Where(x => x.isActive == 0).ToList();
                return View(gdb.Products.Where(x => x.isActive == 0).ToList());
            }
            else
            {
                var userEmail = User.Identity.Name;
                var user = gdb.Users.FirstOrDefault(x => x.email == userEmail);
                if (user.managedStore != null)
                {
                    var store = gdb.Stores.FirstOrDefault(x => x.id == user.managedStore);
                    var ctg = gdb.Categories.Where(x => x.storeID == store.id && x.isActive == 0).ToList();
                    ViewBag.Categories = ctg;
                    List<Products> p = new List<Products>();

                    foreach (var c in ctg)
                    {
                        var products = from prod in gdb.Products
                                       where prod.categoryID == c.id && prod.isActive == 0
                                       select prod;

                        p.AddRange(products);
                    }
                    return View(p);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        public ActionResult Orders()
        {
            if (User.IsInRole("A"))
            {
                return View(gdb.Orders.Where(x => x.isActive == 0).ToList());
            }
            else
            {
                var userEmail = User.Identity.Name;
                var user = gdb.Users.FirstOrDefault(x => x.email == userEmail);
                if (user.managedStore != null)
                {
                    var store = gdb.Stores.FirstOrDefault(x => x.id == user.managedStore);
                    return View(gdb.Orders.Where(x => x.storeID == store.id && x.isActive == 0).ToList());
                }
                else
                {
                    return View("Error");
                }
            }
        }

        public ActionResult Couriers()
        {
            var userEmail = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(x => x.email == userEmail);
            if (User.IsInRole("A") || user.managedStore != null)
            {
                return View(gdb.Couriers.Where(x => x.isActive == 0).ToList());
            }
            else
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "A")]
        public ActionResult Inquiries()
        {
            return View(gdb.Inquiries.Where(x => x.isActive == 0).ToList());
        }

        [Authorize(Roles = "A")]
        [HttpPost]
        public string DeleteUser(int id)
        {
            Users u = gdb.Users.FirstOrDefault(x => x.id == id);
            try
            {
                u.isActive = 1;
                gdb.SaveChanges();
                return "successful";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }



        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult AddUser(Users user)
        {
            bool isAvailable = gdb.Users.Any(u => u.email == user.email && u.id != user.id);
            if (isAvailable)
            {
                TempData["EmailError"] = "The email address you've entered is already in use. Please choose another one.";
                return RedirectToAction("Users");
            }

            gdb.Users.AddOrUpdate(user);
            gdb.SaveChanges();

            var oldUser = gdb.Users.FirstOrDefault(x => x.id == user.id);
            if (oldUser != null && user.cityID != oldUser.cityID)
            {
                var cartItems = gdb.CartItems.Where(x => x.cartID == user.cartID).ToList();
                gdb.CartItems.RemoveRange(cartItems);
            }

            if (user.cartID == null)
            {
                var c = new Carts { userID = user.id };
                gdb.Carts.Add(c);
                user.cartID = c.id;
            }
            if (user.roles == null)
            {
                user.roles = "C";
            }

            gdb.SaveChanges();
            return RedirectToAction("Users");
        }

        [Authorize(Roles = "A")]
        [HttpPost]
        public string DeleteCity(int id)
        {
            Cities c = gdb.Cities.FirstOrDefault(x => x.id == id);
            try
            {
                c.isActive = 1;
                var stores = gdb.Stores.Where(x => x.cityID == c.id).ToList();
                foreach (var st in stores)
                {
                    st.isActive = 1;
                    var categories = gdb.Categories.Where(x => x.storeID == st.id).ToList();
                    foreach (var ct in categories)
                    {
                        ct.isActive = 1;
                        var products = gdb.Products.Where(x => x.categoryID == ct.id).ToList();
                        foreach (var pr in products)
                        {
                            pr.isActive = 1;
                            var cartitems = gdb.CartItems.Where(x => x.productID == pr.id).ToList();
                            gdb.CartItems.RemoveRange(cartitems);
                        }
                    }
                }

                var userCity = gdb.Users.Where(x => x.cityID == id).ToList();
                foreach (var user in userCity)
                {
                    user.cityID = null;
                }

                gdb.SaveChanges();
                return "successful";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult AddCity(Cities c)
        {   
            gdb.Cities.AddOrUpdate(c);
            gdb.SaveChanges();
            return RedirectToAction("Cities");
        }

        [Authorize(Roles = "A")]
        [HttpPost]
        public string DeleteInquiry(int id)
        {
            Inquiries i = gdb.Inquiries.FirstOrDefault(x => x.id == id);
            try
            {
                i.isActive = 1;
                gdb.SaveChanges();
                return "successful";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        [HttpPost]
        public string DeleteStore(int id)
        {
            Stores st = gdb.Stores.FirstOrDefault(x => x.id == id);
            try
            {
                st.isActive = 1;
                var categories = gdb.Categories.Where(x => x.storeID == st.id).ToList();
                foreach (var ct in categories)
                {
                    ct.isActive = 1;
                    var products = gdb.Products.Where(x => x.categoryID == ct.id).ToList();
                    foreach (var pr in products)
                    {
                        pr.isActive = 1;
                        var cartitems = gdb.CartItems.Where(x => x.productID == pr.id).ToList();
                        gdb.CartItems.RemoveRange(cartitems);
                    }
                }
                gdb.SaveChanges();
                return "successful";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult AddStore(Stores st)
        {
            gdb.Stores.AddOrUpdate(st);
            gdb.SaveChanges();
            return RedirectToAction("Stores");
        }

        [HttpPost]
        public string DeleteCategory(int id)
        {
            Categories ct = gdb.Categories.FirstOrDefault(x => x.id == id);
            try
            {
                ct.isActive = 1;
                var products = gdb.Products.Where(x => x.categoryID == ct.id).ToList();
                foreach (var pr in products)
                {
                    pr.isActive = 1;
                    var cartitems = gdb.CartItems.Where(x => x.productID == pr.id).ToList();
                    gdb.CartItems.RemoveRange(cartitems);
                }
                gdb.SaveChanges();
                return "successful";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        [HttpPost]
        public ActionResult AddCategory(Categories ct)
        {
            gdb.Categories.AddOrUpdate(ct);
            gdb.SaveChanges();
            return RedirectToAction("Categories");
        }

        [HttpPost]
        public string DeleteProduct(int id)
        {
            Products pr = gdb.Products.FirstOrDefault(x => x.id == id);
            try
            {
                pr.isActive = 1;
                var cartitems = gdb.CartItems.Where(x => x.productID == pr.id).ToList();
                gdb.CartItems.RemoveRange(cartitems);
                gdb.SaveChanges();
                return "successful";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        [HttpPost]
        public ActionResult AddProduct(Products pr)
        {
            gdb.Products.AddOrUpdate(pr);
            gdb.SaveChanges();
            return RedirectToAction("Products");
        }

        [HttpPost]
        public string DeleteOrder(int id)
        {
            Orders or = gdb.Orders.FirstOrDefault(x => x.id == id);
            try
            {
                or.isActive = 1;
                gdb.SaveChanges();
                return "successful";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        [HttpPost]
        public ActionResult ChangeStatus(int id, string status)
        {
            var order = gdb.Orders.FirstOrDefault(o => o.id == id);
            order.status = status;
            gdb.SaveChanges();
            return RedirectToAction("Orders");
        }

        [Authorize(Roles = "A")]
        [HttpPost]
        public string DeleteCourier(int id)
        {
            Couriers cour = gdb.Couriers.FirstOrDefault(x => x.id == id);
            try
            {
                cour.isActive = 1;
                gdb.SaveChanges();
                return "successful";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult AddCourier(Couriers cour)
        {
            gdb.Couriers.AddOrUpdate(cour);
            gdb.SaveChanges();
            return RedirectToAction("Couriers");
        }
    }
}