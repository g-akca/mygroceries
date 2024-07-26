using GroceryDeliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroceryDeliverySystem.Controllers
{
    public class StoreController : Controller
    {
        groceryDBEntities gdb = new groceryDBEntities();
        // GET: Store
        public ActionResult Index(int id)
        {
            var categories = gdb.Categories.Where(x => x.storeID == id).ToList();
            return View(categories);
        }

        public ActionResult Category(int ctgid)
        {
            var products = gdb.Products.Where(x => x.categoryID == ctgid).ToList();
            var ctg = gdb.Categories.FirstOrDefault(x => x.id == ctgid);
            var categories = gdb.Categories.Where(x => x.storeID == ctg.storeID).ToList();

            ViewBag.Categories = categories;
            return View(products);
        }
    }
}