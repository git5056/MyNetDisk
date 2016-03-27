using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using NetDiskService;

namespace NetDiskWeb
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }


        void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            var id = Context.User.Identity as FormsIdentity;
            if (id != null && id.IsAuthenticated)
            {
                var roles = id.Ticket.UserData.Split(',');
                Context.User = new GenericPrincipal(id, roles);
            }
        }

        /// <summary>
        /// Authen right for user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (Context.User != null)
            {
                IIdentity id = Context.User.Identity;
                
                if (id.IsAuthenticated)
                {
                    try
                    {
                        var iof = Spring.Context.Support.ContextRegistry.GetContext();
                        IRoleToUserService roleService = iof.GetObject("RoleToUserService") as IRoleToUserService;
                        string[] roles = roleService.GetRolesByUserId(Int32.Parse(id.Name)); //new UserRepository().GetRoles(id.Name);
                        Context.User = new GenericPrincipal(id, roles);
                    }
                    catch
                    {
                        //ignore
                    }
                }
            }

            ////var formsIdentity = HttpContext.Current.User.Identity as FormsIdentity;
            ////GenericPrincipal gp = new GenericPrincipal();
            //if (HttpContext.Current.User != null)
            //{
            //    if (HttpContext.Current.User.Identity.IsAuthenticated)
            //    {
            //        if (HttpContext.Current.User.Identity is FormsIdentity)
            //        {
            //            //Get current user identitied by forms
            //            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
            //            // get FormsAuthenticationTicket object
            //            FormsAuthenticationTicket ticket = id.Ticket;
            //            string userData = ticket.UserData;
            //            string[] roles = userData.Split(',');
            //            // set the new identity for current user.
            //            HttpContext.Current.User = new GenericPrincipal(id, roles);
            //        }
            //    }
            //}
        }
        
    }
}