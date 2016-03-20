using NetDiskDomain;
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
            var te2 = iof.GetObject("NodeTreeService") as INodeTreeService;
            //te2.FilterByContentType("6c750233ba004eaeac6db8d828cdfae8", "text");
            //Console.ReadLine();
            var te = iof.GetObject("UserService") as IUserService;
       

            //NetDiskDomain.UserZero ub = new NetDiskDomain.UserZero(null) { enabled = true, userId = "user3", userPwd = "pwd3" };
            //NetDiskDomain.FileSource fs = new NetDiskDomain.FileSource() { content_type="text",deleted=false,enabled=true,isLocal=true,md5="aa",path="aa",postfix=".aa" };
            //var uz = te.FindById(2);
            
            //var s = te.Log("1ef78fb8d872f4bd992ae0a0176f2a8fc", "user2", "pwd2");
            //te.AddNode("6c750233ba004eaeac6db8d828cdfae8", "testdir1", 1,NodeTree.DIR_NODE_ID);
            //te.RmoveFile("6c750233ba004eaeac6db8d828cdfae8", 13);

            //te.DownloadFile("6c750233ba004eaeac6db8d828cdfae8", 17, x => true);

            //te.Save(ub);
        }

    }
}
