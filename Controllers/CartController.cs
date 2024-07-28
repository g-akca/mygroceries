using GroceryDeliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Xml.Linq;
using System.Data.Entity.Migrations;

namespace GroceryDeliverySystem.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        groceryDBEntities gdb = new groceryDBEntities();
        // GET: Cart
        public ActionResult Index()
        {
            var userEmail = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(x => x.email == userEmail);
            var cart = gdb.Carts.FirstOrDefault(x => x.userID == user.id);
            return View(gdb.CartItems.Where(x => x.cartID == cart.id).ToList());
        }

        public ActionResult Checkout()
        {
            var userEmail = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(x => x.email == userEmail);
            var cart = gdb.Carts.FirstOrDefault(x => x.userID == user.id);

            ViewBag.User = user;
            return View(gdb.CartItems.Where(x => x.cartID == cart.id).ToList());
        }

        [HttpPost]
        public ActionResult PlaceOrder(Orders o)
        {
            o.date = DateTime.Now;
            var drivers = gdb.Drivers.ToList();
            Random rnd = new Random();
            int index = rnd.Next(drivers.Count);
            o.driverID = drivers[index].id;
            gdb.Orders.Add(o);

            var email = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(x => x.email == email);
            var cartItems = gdb.CartItems.Where(c => c.cartID == user.cartID).ToList();

            if (cartItems.Count == 0)
            {
                return RedirectToAction("Index");
            }

            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItems
                {
                    orderID = o.id,
                    productID = cartItem.productID,
                    quantity = cartItem.quantity,
                    price = cartItem.price
                };
                o.OrderItems.Add(orderItem);
            }

            gdb.CartItems.RemoveRange(cartItems);
            gdb.SaveChanges();
            return RedirectToAction("OrderSuccessful", o);
        }

        public ActionResult OrderSuccessful(Orders o)
        {
            ViewBag.OrderItems = gdb.OrderItems.Where(x => x.orderID == o.id).ToList();
            ViewBag.CityName = gdb.Cities.FirstOrDefault(x => x.id == o.cityID).name;
            return View(o);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult AddToCart(int productID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, notAuth = true });
            }
            var email = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(x => x.email == email);
            var cartID = user.cartID.Value;
            var product = gdb.Products.FirstOrDefault(x => x.id == productID);

            var existingCartItem = gdb.CartItems.FirstOrDefault(x => x.cartID == cartID);
            bool isSameStore = true;

            if (existingCartItem != null)
            {
                var existingProductCtgID = gdb.Products.FirstOrDefault(x => x.id == existingCartItem.productID).categoryID;
                var existingProductCtg = gdb.Categories.FirstOrDefault(x => x.id == existingProductCtgID);
                var existingProductStore = gdb.Stores.FirstOrDefault(x => x.id == existingProductCtg.storeID);

                var newProductCtgID = product.categoryID;
                var newProductCtg = gdb.Categories.FirstOrDefault(x => x.id == newProductCtgID);
                var newProductStore = gdb.Stores.FirstOrDefault(x => x.id == newProductCtg.storeID);

                if (existingProductStore != newProductStore)
                {
                    isSameStore = false;
                }
            }

            if (isSameStore)
            {
                var cartItem = gdb.CartItems.FirstOrDefault(x => x.cartID == cartID && x.productID == productID);

                if (cartItem != null)
                {
                    cartItem.quantity++;
                    cartItem.price += product.price;
                }
                else
                {
                    cartItem = new CartItems
                    {
                        productID = productID,
                        quantity = 1,
                        cartID = cartID,
                        price = product.price
                    };
                    gdb.CartItems.Add(cartItem);
                }

                gdb.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Your cart has products from another store!" });
            }
        }

        [HttpPost]
        public void DeleteFromCart(int cartitemID)
        {
            var cartItem = gdb.CartItems.FirstOrDefault(x => x.id == cartitemID);
            gdb.CartItems.Remove(cartItem);
            gdb.SaveChanges();
        }

        [HttpPost]
        public void AddQuantity (int cartitemID)
        {
            var cartItem = gdb.CartItems.FirstOrDefault(x => x.id == cartitemID);
            cartItem.quantity++;
            cartItem.price += cartItem.Products.price;
            gdb.CartItems.AddOrUpdate(cartItem);
            gdb.SaveChanges();
        }

        [HttpPost]
        public void ReduceQuantity(int cartitemID)
        {
            var cartItem = gdb.CartItems.FirstOrDefault(x => x.id == cartitemID);
            cartItem.quantity--;
            cartItem.price -= cartItem.Products.price;
            if (cartItem.quantity == 0)
            {
                gdb.CartItems.Remove(cartItem);
            }
            else
            {
                gdb.CartItems.AddOrUpdate(cartItem);
            }
            gdb.SaveChanges();
        }
    }
}