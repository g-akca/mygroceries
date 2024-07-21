using GroceryDeliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}