using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetDiskService;
using System.IO;
//using NetDiskDomain;

namespace NetDiskWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Index/

        public ActionResult Index()
        {
            return View();
        }

        public static NetDiskDomain.IUserRunTime CurrentUser()
        {
            var iof = Spring.Context.Support.ContextRegistry.GetContext();
            var sessionService = iof.GetObject("SessionService") as ISessionService;
            var currentUser = sessionService.GetCurrentUser(System.Web.HttpContext.Current.Session["sessionid"] == null ? Guid.NewGuid().ToString("N") : Convert.ToString(System.Web.HttpContext.Current.Session["sessionid"]));
            return currentUser;
        }

        public ActionResult UploadFile(int ? nodeId)
        {
            ViewBag.nodeId = nodeId.HasValue && nodeId.Value > 0 ? nodeId.Value : -1;
            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase upfile)
        {

            //在写这里的时候突然想到了一个bug，就是MD5相同的话还不行，这样别人拿MD5试着秒传，会不会窃取到别人的私人文件
            //所以还应该加上其他信息，一起验证，如文件大小，类型等
            //待改进
            var md5 = Convert.ToString(Request["md5"]);
            var nodeId = Convert.ToInt32(Request["nodeId"]);
            var fileName = upfile==null?"": upfile.FileName;
            if ( string.IsNullOrWhiteSpace(md5)||string.IsNullOrWhiteSpace(fileName))
            {
                return new JsonResult() { Data = new { code = 0, msg = "failure,请先选择文件" } };
            }
            try
            {
                if (upfile != null && upfile.ContentLength > 0)
                {

                    var iof = Spring.Context.Support.ContextRegistry.GetContext();
                    var userService = iof.GetObject("UserService") as IUserService;


                    try
                    {
                        //UpFile
                        var sessionId=Session["sessionid"] != null ? Convert.ToString(Session["sessionid"]) : new Guid().ToString("N");
                        object a=null;
                        userService.UploadFile(sessionId,(out object o) => {
                          
                            o = new NetDiskDomain.FileUploadContextInfo();
                            NetDiskDomain.FileUploadContextInfo co = o as NetDiskDomain.FileUploadContextInfo;
                            co.FileName = fileName;
                            co.FilePath = "/content/files/" +  DateTime.Now.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString("N") + Path.GetExtension(upfile.FileName); ;      
                            co.ContentType = upfile.ContentType;
                            co.IsLocal = true;
                            co.MD5 = md5;
                            co.PostFix = Path.GetExtension(fileName);
                            co.ParentNodeId = nodeId;
                            a=o;
                            return true;
                        });
                        //是否秒传
                        var c=a as NetDiskDomain.FileUploadContextInfo;
                        if (!c.IsFlash)
                        {
                            var filePath = Path.Combine(Server.MapPath("~/Content/Files"), Path.GetFileName(c.FilePath));
                            upfile.SaveAs(filePath);
                        }
                        return new JsonResult() { Data = new { code = 0, msg = "success,请刷新页面在该节点下查看", flashUpload = c.IsFlash } };
                    }
                    catch
                    {
                        return new JsonResult() { Data = new { code = 1, msg = "failure" } };
                    }
                }
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

    }
}
