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

        [HttpPost]
        public void AddToCart(int productID)
        {
            var email = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(x => x.email == email);
            var cartID = user.cartID.Value;
            var product = gdb.Products.FirstOrDefault(x => x.id == productID);

            if (gdb.CartItems.FirstOrDefault(x => x.cartID == cartID && x.productID == productID) != null)
            {
                gdb.CartItems.FirstOrDefault(x => x.cartID == cartID && x.productID == productID).quantity++;
                gdb.CartItems.FirstOrDefault(x => x.cartID == cartID && x.productID == productID).price += product.price;
                gdb.SaveChanges();
            }
            else
            {
                var cartItem = new CartItems
                {
                    productID = productID,
                    quantity = 1,
                    cartID = cartID,
                    price = product.price
                };

                gdb.CartItems.Add(cartItem);
                gdb.SaveChanges();
            }
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