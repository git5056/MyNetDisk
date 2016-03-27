using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetDiskDomain;

namespace NetDiskWeb
{
    public class AuthorizeFilter : AuthorizeAttribute
    {
        public AuthorizeFilter(IUserRunTime user){

        }

        public AuthorizeFilter(string sessionId)
        {

        }

        public AuthorizeFilter(Func<IUserRunTime, bool> pass)
        {

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var bv = httpContext.User.IsInRole("aaaaa");

            return base.AuthorizeCore(httpContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }

        #region helper



        #endregion
    }
}