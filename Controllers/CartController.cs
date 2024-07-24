using GroceryDeliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace GroceryDeliverySystem.Controllers
{
    [Authorize] //[Authorize(Roles="A,S,C")]
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
    }
}