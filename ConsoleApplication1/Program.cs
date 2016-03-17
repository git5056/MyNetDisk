using NetDiskService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = Spring.Context.Support.ContextRegistry.GetContext();
            var iof = context;
            var te = iof.GetObject("SessionService") as ISessionService;

            //NetDiskDomain.UserZero ub = new NetDiskDomain.UserZero(null) { enabled = true, userId = "user3", userPwd = "pwd3" };
            //NetDiskDomain.FileSource fs = new NetDiskDomain.FileSource() { content_type="text",deleted=false,enabled=true,isLocal=true,md5="aa",path="aa",postfix=".aa" };
            var a= te.FindById(2);
            var I = te.GetCurrentUser();

            //te.Save(ub);
        }
    }
}
