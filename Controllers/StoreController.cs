using GroceryDeliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace GroceryDeliverySystem.Controllers
{
    public class StoreController : Controller
    {
        groceryDBEntities gdb = new groceryDBEntities();
        // GET: Store
        public ActionResult Index(int id)
        {
            var categories = gdb.Categories.Where(x => x.storeID == id).ToList();
            var store = gdb.Stores.FirstOrDefault(x => x.id == id);
            ViewBag.Store = store;
            return View(categories);
        }

        public ActionResult Category(int ctgid)
        {
            var products = gdb.Products.Where(x => x.categoryID == ctgid).ToList();
            var ctg = gdb.Categories.FirstOrDefault(x => x.id == ctgid);
            var categories = gdb.Categories.Where(x => x.storeID == ctg.storeID).ToList();
            var store = gdb.Stores.FirstOrDefault(x => x.id == ctg.storeID);

            ViewBag.Categories = categories;
            ViewBag.Store = store;
            ViewBag.CategoryName = ctg.name;
            return View(products);
        }
    }
}