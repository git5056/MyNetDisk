using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;

namespace NetDiskRepository
{

    public interface IUserRepository:IRepository<UserZero>
    {
        #region Extension Methods

        Session Log(Vistor visitor,string userId,string userPwd);

        void DownloadFile(IFileDownload user, NodeTree node ,Func<string,bool> doDown);

        void UploadFile(IFileUpload user, DoUpHandle doUp);

        void AddNode(UserZero uz, string name, int pNodeId,int fsId);

        void RmoveNode(UserZero uz,int nodeId);

        void RenameNode(UserZero uz, int childId, string name);

        void MoveNode(UserZero uz, int parentId, int nodeId);

        #endregion
    }
}
