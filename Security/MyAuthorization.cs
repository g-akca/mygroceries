using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroceryDeliverySystem.Security
{
    public class MyAuthorization:AuthorizeAttribute
    {
        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
            
        //    if(this.AuthorizeCore(filterContext.HttpContext))
        //    {
        //        base.OnAuthorization(filterContext);
        //    }
        //    else
        //    {
        //        filterContext.Result = new RedirectResult("/Home/Index/");
        //    }
        //}

        private string[] AuthorizedRoles { get; set; }

        public MyAuthorization(params string[] autRoles)
        {
            this.AuthorizedRoles = autRoles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User;

            // Visitors can enter
            if (!user.Identity.IsAuthenticated)
            {
                return true;
            }

            foreach (var role in AuthorizedRoles)
            {
                if (user.IsInRole(role))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Home/Index/");
        }
    }
}