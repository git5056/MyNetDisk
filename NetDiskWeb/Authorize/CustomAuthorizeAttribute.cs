using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetDiskWeb
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
       
        public new string[] Roles { get; set; }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            if (Roles == null)
            {
                return true;
            }
            if (Roles.Length == 0)
            {
                return true;
            }
            if (httpContext.User.IsInRole("superadministrator"))
            {
                return true;
            }
            if (Roles.Any(httpContext.User.IsInRole))
            {
                return true;
            }
            return false;
        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            string roles = GetRoles.GetActionRoles(actionName, controllerName);
            if (!string.IsNullOrWhiteSpace(roles))
            {
                this.Roles = roles.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.Write("<script>alert('权限不足');</script>");
            //filterContext.HttpContext.Response.Redirect("/home/index");
            filterContext.HttpContext.Response.End();
            //base.HandleUnauthorizedRequest(filterContext);
        }

    }

}