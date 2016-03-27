using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetDiskService;
using System.IO;
using NetDiskDomain;
using System.Configuration;
using System.Xml;
using System.Web.Security;

namespace NetDiskWeb.Controllers
{

    public class HomeController : Controller
    {
        //
        // GET: /Index/

        #region IService Property
        public IUserService UserService
        {
            get
            {
                var iof = Spring.Context.Support.ContextRegistry.GetContext();
                return iof.GetObject("UserService") as IUserService;
            }
        }

        public ISessionService SessionService
        {
            get
            {
                //这里可能还存在一个略显蛋疼的问题，就是Spring.Context.Support.ContextRegistry.GetContext();出现
                //异常时，他打死都不跳进catch语句里，后来不知怎么弄了下他又可以捕捉到了
                //我不知道在别人的机器上是否会出现这个问题
                //如果出现了就自己手动跳到http://localhost:????/Home/DBConfig
                //try
                //{
                    var iof = Spring.Context.Support.ContextRegistry.GetContext();
                    return iof.GetObject("SessionService") as ISessionService;
                //}
                //catch
                //{
                    
                //}
                //finally
                //{
              
                //}
                //return null;
            }
        }

        #endregion

        public ActionResult LogOn(string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Session["aa"] == null)
                {
                    //HttpContext.User = new APrincipal("aaaaaaaaa");
                    var role = "Admine";
                    Session["aa"] = role;

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                               "Adminb",
                                DateTime.Now,
                                DateTime.Now.AddMinutes(30),
                                false,
                                null);
                    //string encTicket = FormsAuthentication.Encrypt(authTicket);
                    //this.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    FormsAuthentication.SetAuthCookie("Admin", true);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            // If we got this far, something failed, redisplay form
            return null;
        }

        #region Action

        //[HandleError(View = "DBConfig", ExceptionType = typeof(Exception))]

        //[CustomAuthorizeAttribute]
        public ActionResult Index()
        {
            try
            {
                return View(CurrentUser);
            }
            //跳转到数据库配置
            catch (ConfigurationErrorsException e)
            {
                return RedirectToAction("DBConfig");
            }
        }


        [HttpGet]
        public ActionResult DBConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("~/Web.config"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("databaseSettings");
            //
            //<add key="db.datasource" value="to_heart2-pc" />
            ViewBag.datasource = node.ChildNodes[0].Attributes["value"].Value;
            //<add key="db.user" value="sa" />
            ViewBag.user = node.ChildNodes[1].Attributes["value"].Value;
            //<add key="db.password" value="1111" />
            ViewBag.password = node.ChildNodes[2].Attributes["value"].Value;
            //<add key="db.database" value="MyNetDisk" />
            ViewBag.database = node.ChildNodes[3].Attributes["value"].Value;
            return View();
        }

        [HttpPost]
        public ActionResult DBConfig(string nop)
        {
            var data_source = Request["datasource"];
            var user = Request["user"];
            var pwd = Request["password"];
            var database = Request["database"];

            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("~/Web.config"));
            XmlNode node = doc.DocumentElement.SelectSingleNode("databaseSettings");
            //
            //<add key="db.datasource" value="to_heart2-pc" />
            node.ChildNodes[0].Attributes["value"].Value = data_source;
            //<add key="db.user" value="sa" />
            node.ChildNodes[1].Attributes["value"].Value = user;
            //<add key="db.password" value="1111" />
            node.ChildNodes[2].Attributes["value"].Value = pwd;
            //<add key="db.database" value="MyNetDisk" />
            node.ChildNodes[3].Attributes["value"].Value = database;

            doc.DocumentElement.SelectSingleNode("databaseSettings").InnerXml = node.InnerXml;
            doc.Save(Server.MapPath("~/Web.config"));
            return Redirect("/");
        }

        [CustomAuthorizeAttribute]
        [HttpGet]
        public ActionResult FileUpload(int? nodeId)
        {
            ViewBag.nodeId = nodeId.HasValue && nodeId.Value > 0 ? nodeId.Value : 0;
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase upfile)
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
                    try
                    {
                        //UpFile
                        var sessionId=Session["sessionid"] != null ? Convert.ToString(Session["sessionid"]) : new Guid().ToString("N");
                        object context=null;
                        UserService.UploadFile(sessionId,(out object o) => {
                          
                            o = new NetDiskDomain.FileUploadContextInfo();
                            NetDiskDomain.FileUploadContextInfo co = o as NetDiskDomain.FileUploadContextInfo;
                            //Fill Data
                            co.FileName = fileName;
                            co.FilePath = "/content/files/" +  DateTime.Now.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString("N") + Path.GetExtension(upfile.FileName); ;      
                            co.ContentType = upfile.ContentType;
                            co.IsLocal = true;
                            co.MD5 = md5;
                            co.PostFix = Path.GetExtension(fileName);
                            co.ParentNodeId = nodeId;
                            context=o;
                            return true;
                        });   
                        var tmp=context as NetDiskDomain.FileUploadContextInfo;
                        //是否秒传
                        if (!tmp.IsFlash)
                        {
                            var filePath = Path.Combine(Server.MapPath("~/Content/Files"), Path.GetFileName(tmp.FilePath));
                            upfile.SaveAs(filePath);
                        }
                        return new JsonResult() { Data = new { code = 0, msg = "success,请刷新页面在该节点下查看", flashUpload = tmp.IsFlash } };
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

        #endregion

        #region Helper Methods

        public IUserRunTime CurrentUser
        {
            get
            {
                return SessionService.GetCurrentUser(GetSessionId());
            }
        }

        private string GetSessionId()
        {
            return Session["sessionid"] != null ? Convert.ToString(Session["sessionid"]) : new Guid().ToString("N");
        }

        #endregion

    }
}
