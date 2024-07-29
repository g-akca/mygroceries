using GroceryDeliverySystem.Models;
using GroceryDeliverySystem.Security;
using System.Linq;
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
            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;
                var user = gdb.Users.FirstOrDefault(x => x.email == email);
                return View(gdb.Stores.Where(x => x.cityID == user.cityID && x.isActive == 0).ToList());
            }
            else
            {
                return View(gdb.Stores.Where(x => x.isActive == 0).ToList());
            }
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

            if (us != null && us.isActive == 0)
            {
                FormsAuthentication.SetAuthCookie(u.email, false);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Incorrect email or password!";
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
            bool isAvailable = gdb.Users.Any(user => user.email == u.email);
            if (isAvailable)
            {
                TempData["EmailError"] = "The email address you've entered is already in use. Please choose another one.";
                return RedirectToAction("Signup");
            }
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
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            bool exists = gdb.Users.Any(u => u.email == email);
            if (exists)
            {
                TempData["EmailSuccess"] = "We have sent you an email to reset your password. Please check your inbox.";
            }
            else
            {
                TempData["EmailError"] = "We couldn't find the email address you've entered. Please try again.";
            }
            return RedirectToAction("ForgotPassword");
        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(Inquiries i)
        {
            TempData["Success"] = "Thank you for contacting us. We will get back to you soon.";
            gdb.Inquiries.Add(i);
            gdb.SaveChanges();
            return RedirectToAction("ContactUs");
        }

        [HttpGet]
        [Authorize]
        public ActionResult NavBar1()
        {
            var userEmail = User.Identity.Name;
            var user = gdb.Users.FirstOrDefault(u => u.email == userEmail);
            var cart = gdb.Carts.FirstOrDefault(c => c.userID == user.id);
            var cartItems = gdb.CartItems.Where(x => x.cartID == cart.id).ToList();

            ViewBag.User = user;
            ViewBag.Cities = gdb.Cities.Where(x => x.isActive == 0).ToList();

            return PartialView("_NavBar1", cartItems);
        }
    }
}