using NetDiskDomain;
using NetDiskService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NetDiskWeb.Controllers
{
    public class AjaxController : Controller
    {

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
                var iof = Spring.Context.Support.ContextRegistry.GetContext();
                return iof.GetObject("SessionService") as ISessionService;
            }
        }

        public INodeTreeService NodeService
        {
            get
            {
                var iof = Spring.Context.Support.ContextRegistry.GetContext();
                return iof.GetObject("NodeTreeService") as INodeTreeService;
            }
        }

        #endregion

        public IUserRunTime CurrentUser
        {
            get
            {
                return SessionService.GetCurrentUser(GetSessionId());
            }
        }

        #region Action

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Lazy(int ? nodeId)
        {
            if (CurrentUser is UserZero)
            {
                var user = CurrentUser as UserZero;
                //user.RootNode
                //var query = for tmp in user.RootNode.GetChildInAll(nodeId.Value) gr
                //var result = ToJson(user.RootNode.GetChildInAll(nodeId.Value));
                var aimNode = user.RootNode.GetChildInAll(nodeId.Value);
                var query = from node in aimNode.ChildNodes where !node.IsRemoved() orderby node.IsFolderNode() descending, node.name ascending select new { id = node._id, text = node.name, children = node.IsFolderNode() && node.ChildNodes.Count > 0 ? new object[] { " place holder" } : null, state="closed", icon = !node.IsFolderNode() ? "jstree-file" : "jstree-folder" };
                var jResult = new JsonResult();
                jResult.Data = query.ToArray();
                jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jResult;
            }
            else
                return null;
        }

        public JsonResult DwonRecond()
        {   
            if (CurrentUser is UserZero)
            {
                var user = CurrentUser as UserZero;
                JsonResult jr = new JsonResult();
                var jslist = new List<object>();
                //按照时间倒序查询
                var query = from tmp in user.DownloadReconds orderby tmp.downloadTime descending select tmp;
                foreach (var i in query)
                {
                    jslist.Add(new { time = i.downloadTime.ToString("yyyy/MM/dd/HH:mm:ss"), name = i.Node.name });
                }
                jr.Data = jslist;
                return jr;
            }
            else
                //返回空数组
                return new JsonResult{ Data=new List<object>() };

        }

        public JsonResult DownRank()
        {

            if (CurrentUser is UserZero)
            {
                var user = CurrentUser as UserZero;
                JsonResult jr = new JsonResult();
                var jslist = new List<object>();
                
                var query = from tmp in user.DownloadReconds group tmp by tmp.Node into g orderby g.Count() descending select new { g.Key, count=g.Count() } ;
                foreach (var i in query)
                {
                    jslist.Add(new { name = i.Key.name, count = i.count });
                }
                jr.Data = jslist;
                return jr;
            }
            else
                //返回空数组
                return new JsonResult { Data = new List<object>() };
        }

        public JsonResult CreateDir(int? pId, string name)
        {
            if (!pId.HasValue)
            {
                return new JsonResult() { Data = new { code = 1 } };
            }
            try
            {
                UserService.AddNode(GetSessionId(), name, pId.Value, Node.FOLDER_NODE_ID);
                //需要返回一个id来修改节点值

                var pNode = (CurrentUser as UserZero).RootNode.GetChildInAll(pId.Value);
                foreach (var i in pNode.ChildNodes)
                {
                    if (!i.IsRemoved() && i.name == name)
                    {
                        return new JsonResult() { Data = new { code = 0, nodeId = i._id } };
                    }
                }
                return new JsonResult() { Data = new { code = 1 } };
            }
            catch
            {
                return new JsonResult() { Data = new { code = 1 } };
            }
        }

        public JsonResult DeleteNode(int? nodeId)
        {
            if (!nodeId.HasValue)
            {
                return new JsonResult() { Data = new { code = 1 } };
            }
            try
            {
                UserService.RmoveNode(GetSessionId(), nodeId.Value);
                return new JsonResult() { Data = new { code = 0 } };
            }
            catch
            {
                return new JsonResult() { Data = new { code = 1 } };
            }

        }

        public JsonResult RenameNode(int? nodeId, string newname)
        {
            if (!nodeId.HasValue || string.IsNullOrEmpty(newname))
            {
                return new JsonResult() { Data = new { code = 1 } };
            }
            try
            {
                UserService.RenameNode(GetSessionId(), nodeId.Value, newname);
                return new JsonResult() { Data = new { code = 0 } };
            }
            catch
            {
                return new JsonResult() { Data = new { code = 1,msg="" } };
            }
        }

        public JsonResult MoveNode(int? parentId, int? nodeId)
        {
            if (!parentId.HasValue || !nodeId.HasValue)
            {
                return new JsonResult() { Data = new { code = 1 } };
            }
            try
            {
                UserService.MoveNode(GetSessionId(), parentId.Value, nodeId.Value);
                //返回nodeId可能会用到
                return new JsonResult() { Data = new { code = 0 ,parentId = parentId } };
            }
            catch
            {
                return new JsonResult() { Data = new { code = 1, msg = "" } };
            }
        }

        public FileResult DownFile(int? nodeId)
        {
            
            if (!nodeId.HasValue)
            {
                return null;
            }
            var fileName="";
            var filePath = "";
            try
            { 
                UserService.DownloadFile(
                    GetSessionId(),
                    nodeId.Value,
                    (path) =>
                    {
                        filePath = Server.MapPath(path);
                        fileName = Path.GetFileName(filePath);
                        return true;
                    });
                    //我在想，难道mvc就没有提供一个异步或者有回调的下载文件的方法吗,导致我的设计就不对，我想的是在下载完后才想数据库写入记录
                return File(filePath, "application/octet-stream", fileName);
            }
            catch
            {
                return null;
            }
        }

        public JsonResult FilterFile(string content)
        {
            var jr = new JsonResult();
            jr.Data = new List<object>();
            if (content == null || string.IsNullOrEmpty(content.Trim()))
            {
                return jr;
            }
            var iof = Spring.Context.Support.ContextRegistry.GetContext();
            var nodeTreeService = iof.GetObject("NodeTreeService") as INodeTreeService;
            try
            {
                var nodes = nodeTreeService.FilterByContentType(GetSessionId(), content);

                var data = new List<object>();
                foreach (var i in nodes)
                {
                    var tmp = new { id=i._id,name=i.name };
                    data.Add(tmp);
                }
                jr.Data = data;
                return jr;
            }
            catch
            {
                return jr;
            }
        }

        public JsonResult Login(string userId, string userPwd)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(userPwd))
            {
                return new JsonResult() { Data = new { code = 1, msg = "账号或密码为空" } };
            }
            try
            {
                var session = UserService.Log(Session["sessionid"] == null ? Guid.NewGuid().ToString("N") : Convert.ToString(Session["sessionid"]), userId, userPwd);
                Session["sessionid"] = session.sessionId;
                FormsAuthentication.SetAuthCookie(session._User._id.ToString(), /*true*/false);
                return new JsonResult() { Data = new { code = 0, msg = "登录成功" } };
            }
            catch
            {
                return new JsonResult() { Data = new  { code=1,msg="登录失败 " } };
            }
        }

        public RedirectResult LogOff()
        {
            Session["sessionid"] = null;
            //清除权限
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        #endregion

        #region Helper Methods

        private string GetSessionId()
        {
            return Session["sessionid"] != null ? Convert.ToString(Session["sessionid"]) : new Guid().ToString("N");
        }

        #region jstree_json

        //internal class NodeJson
        //{
        //    public string text
        //    {
        //        get;
        //        set;
        //    }

        //    public NodeJsonState state
        //    {
        //        get;
        //        set;
        //    }

        //    public string icon
        //    {
        //        get;
        //        set;
        //    }

        //    public IList<NodeJson> children
        //    {
        //        get;
        //        set;
        //    }

        //    //ex
        //    public int id
        //    {
        //        get;
        //        set;
        //    }

        //}

        //internal class NodeJsonState
        //{
        //    public bool opened
        //    {
        //        get;
        //        set;
        //    }

        //    public bool disabled
        //    {
        //        get;
        //        set;
        //    }
        //}

        //private static void CloneNodeToNodeJson(Node node,NodeJson ntj)
        //{
        //    ntj.text = node.name;
        //    ntj.state = new NodeJsonState() { disabled = false, opened = true };
        //    ntj.icon = !node.IsFolderNode() ? "jstree-file" : "jstree-folder";
        //    ntj.children = new List<NodeJson>();
        //    ntj.id = node._id;
        //    foreach (var i in node.ChildNodes.ToList())
        //    {
        //        //未被逻辑删除
        //        if (i.enabled)
        //        {
        //            var tmpNode = new NodeJson();
        //            tmpNode.id = i._id;
        //            tmpNode.text = i.name;
        //            tmpNode.state = new NodeJsonState() { disabled = false, opened = true };
        //            tmpNode.icon = !i.IsFolderNode() ? "jstree-file" : "jstree-folder";
        //            ntj.children.Add(tmpNode);
        //            CloneNodeToNodeJson(i, tmpNode);
        //        }
        //    }
        //    //return ntj;
        //}

        //private static JsonResult ToJson(Node node){
        //    JsonResult jr = new JsonResult();
        //    NodeJson ntj = new NodeJson();
        //    CloneNodeToNodeJson(node,ntj);
        //    jr.Data = ntj;
        //    return jr;
        //}

        #endregion

        #endregion
    }
}
