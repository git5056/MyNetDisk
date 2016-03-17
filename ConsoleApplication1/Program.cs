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
            var te = iof.GetObject("repository.user") as NetDiskRepository.IUserRepository;
            NetDiskDomain.UserBase ub = new NetDiskDomain.UserBase() { enabled = true, userId = "user2", userPwd = "pwd2" };
            NetDiskDomain.FileSource fs = new NetDiskDomain.FileSource() { content_type="text",deleted=false,enabled=true,isLocal=true,md5="aa",path="aa",postfix=".aa" };

            te.Save(ub);
        }
    }
}
