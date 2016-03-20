using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;

namespace NetDiskRepository
{
    public class UserRepository : Repository<UserZero>, IUserRepository
    {
        public virtual ISessionRepository SessionRepository
        {
            get;
            set;
        }

        public virtual IDownloadRecondRepository DownloadRecondRepository
        {
            get;
            set;
        }

        public virtual INodeTreeRepository NodeTreeRepository
        {
            get;
            set;
        }

        public virtual IFileSourceRepository FileSourceRepository
        {
            get;
            set;
        }

        public Session Log(Vistor visitor ,string userId, string userPwd)
        {
            try
            {
               var newSession = visitor.Log(userId, userPwd, 
                   (x, y) => 
                   FindByHQL("from "+typeof(UserZero)+" where userId=? and userPwd=?",x,y)[0]);
               //persist
               SessionRepository.Save(newSession);
               return newSession;
            }
            catch
            {
                throw new Exception("密码错误，或者账号不存在");
            }
        }


        public void DownloadFile(IFileDownload downloader, NodeTree node, Func<string, bool> doDown)
        {
            if (node.FileSource == null)
            { 
                throw new Exception("资源不存在");
            }
            var newRecond = downloader.DownFile(node, doDown);
            //persist
            DownloadRecondRepository.Save(newRecond);

        }

        public void UploadFile(IFileUpload user, DoUpHandle doUp)
        {
            DoUpHandle d = (out object context) =>
            {
                context = new FileUploadContextInfo();      
                doUp(out context); 
                var co=context as FileUploadContextInfo;
                //创建FileSoruc
                var fs= FileSourceRepository.FindByHQL("from "+typeof(FileSource)+" where @md5=?",co.MD5);
                FileSource persistedFileSource=null;
                //如果存在,秒传
                if(fs.Count>0){
                    co.IsFlash = true;
                }
                    //则创建
                else{
                    FileSourceRepository.Save(new FileSource(){ content_type=co.ContentType,deleted=false,enabled=true,isLocal=true,md5=co.MD5,path=co.FilePath,postfix=co.PostFix });
                    fs = FileSourceRepository.FindByHQL("from " + typeof(FileSource) + " where @md5=?", co.MD5); 
                }
                //持久对象
                persistedFileSource = fs[0];
                co.Fs = persistedFileSource;
                return true;
            };
            user.UpFile(d);
        }

        public void AddNode(UserZero uz, string name, int pNodeId,int fsId)
        {
            var node = uz.RootNode.FindIt(pNodeId);
            if (node != null)
            {
                //persist
                node.AppendChild(name, FileSourceRepository.FindById(fsId));
                Update(uz);
                return;
            }
            throw new Exception("节点不存在");
        }

        public void RmoveNode(UserZero uz, int nodeId)
        {
            uz.RootNode.RemoveChild(nodeId);
            //persist
            SaveOrUpdate(uz);
        }

        public void RenameNode(UserZero uz, int childId, string name)
        {
            uz.RootNode.RenameChild(childId,name);
            //persist
            SaveOrUpdate(uz);
        }


        public void MoveNode(UserZero uz, int parentId, int nodeId)
        {
            uz.RootNode.Move(parentId, nodeId);
            //persist
            SaveOrUpdate(uz);
        }



    }
}
