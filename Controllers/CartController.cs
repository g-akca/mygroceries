using GroceryDeliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroceryDeliverySystem.Controllers
{
    [Authorize] //[Authorize(Roles="A,S,C")]
    public class CartController : Controller
    {
        groceryDBEntities gdb = new groceryDBEntities();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
    }
}