using Iesi.Collections.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetDiskDomain
{
    /// <summary>
    /// 普通用户，之上vip1,vip2等扩展
    /// </summary>
    public class UserZero : AbstractUserRunTime, IFileManager
    {
        public UserZero() { }
        public UserZero(Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            this._Session = session;
        }

        #region public property
        /// <summary>
        /// _id
        /// </summary>
        public virtual int _id
        {
            get;
            set;
        }
        /// <summary>
        /// userId
        /// </summary>
        public virtual string userId
        {
            get;
            set;
        }
        /// <summary>
        /// userPwd
        /// </summary>
        public virtual string userPwd
        {
            get;
            set;
        }
        /// <summary>
        /// enabled
        /// </summary>
        public virtual bool enabled
        {
            get;
            set;
        }

        /// <summary>
        /// rootNodeId
        /// </summary>
        public virtual int rootNodeId
        {
            get;
            set;
        }
        

        private Iesi.Collections.Generic.ISet<DownloadRecond> downloadReconds;

        /// <summary>
        /// DownloadReconds
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<DownloadRecond> DownloadReconds
        {
            get
            {
                if (downloadReconds == null)
                {
                    downloadReconds = new HashedSet<DownloadRecond>();
                }
                return downloadReconds;
            }
            set
            {
                downloadReconds = value;
            }
        }

        private Iesi.Collections.Generic.ISet<UploadRecond> uploadReconds;

        /// <summary>
        /// UploadReconds
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<UploadRecond> UploadReconds
        {
            get
            {
                if (uploadReconds == null)
                {
                    uploadReconds = new HashedSet<UploadRecond>();
                }
                return uploadReconds;
            }
            set
            {
                uploadReconds = value;
            }
        }

        //private Iesi.Collections.Generic.ISet<NodeTree> nodeTrees;

        ///// <summary>
        ///// NodeTrees
        ///// </summary>
        //public virtual Iesi.Collections.Generic.ISet<NodeTree> NodeTrees
        //{
        //    get
        //    {
        //        if (nodeTrees == null)
        //        {
        //            nodeTrees = new HashedSet<NodeTree>();
        //        }
        //        return nodeTrees;
        //    }
        //    set
        //    {
        //        nodeTrees = value;
        //    }
        //}

        /// <summary>
        /// RootNode
        /// </summary>
        public virtual NodeTree RootNode
        {
            get;
            set;
        }

        #endregion

        #region private field

        private Session _session;

        #endregion

        #region public methods

        public virtual bool Own(int fsId)
        {
            return OwnInternal(fsId, RootNode);
        }

        /// <summary>
        /// Recursive Helper 
        /// </summary>
        /// <param name="fsId"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool OwnInternal(int fsId, NodeTree node)
        {
            foreach (NodeTree i in node)
            {
                if (i.FileSource != null && i.FileSource._id == fsId)
                {
                    return true;
                }
            }
            return false;

        }

        public virtual void ClearDownRecond()
        {
            DownloadReconds.Clear();
        }
        public virtual void ClearUpRecond()
        {
            UploadReconds.Clear();
        }


        #region download

        public virtual bool TryDown(object context)
        {
            if (Own((int)context))
            {
                return true;
            }
            return false;
        }

        public virtual DownloadRecond DownFile(NodeTree node, Func<string, bool> doDown)
        {
            //log.....................
            if (node.FileSource == null)
            {
                throw new ArgumentNullException("fs");
            }

            //Validate file Integrity and Security
            //Validate();
            
            if (node.FileSource != null && node.FileSource.enabled && !node.FileSource.deleted && TryDown(node.FileSource._id) && doDown(node.FileSource.path))
            {
                var recond = new DownloadRecond() { downloadTime = DateTime.Now, Node = node, userId = _id };
                this.DownloadReconds.Add(recond);
                return recond;
            }
            else
            {

            }
            return null;
        }

        #endregion 

        #region upload

        /// <summary>
        /// return true
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool TryUp(object context)
        {
            return true;
        }

        public virtual void UpFile(DoUpHandle doUp)
        {
            //这里不做权限控制,返回真
            if (TryUp(null))
            {
                var context = new object();

                if (doUp(out context))
                {
                    //adaptee
                    if (context is FileUploadContextInfo)
                    {
                        var contextInfo = context as FileUploadContextInfo;
                        //writing to nodeTree
                        RootNode.GetChildInAll(contextInfo.ParentNodeId).AppendChild(contextInfo.FileName, contextInfo.Fs);

                        //writing to uploadrecond
                        UploadReconds.Add(new UploadRecond() { uploadTime = DateTime.Now,userId=_id,fileSourceId=contextInfo.Fs._id,flashUp=contextInfo.IsFlash });

                        return;
                    }
                    throw new Exception("无法适配处理对象context");
                }
            }
            else
            {
                throw new Exception("上载失败，权限不够");
            }
        }

        #endregion

        public override Session _Session
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
            }
        }

        protected override int GetUserId()
        {
            return _id;
        }


        #endregion

    }
}
