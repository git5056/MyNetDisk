using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;

namespace NetDiskService
{

    public interface IUserService : IService<UserZero>
    {
        #region Extension Methods

        Session Log(string sessionId, string userId, string userPwd);

        void DownloadFile(string sessionId, int nodeTreeId, Func<string, bool> doDown);

        void UploadFile(string sessionId, DoUpHandle doUp);

        void AddNode(string sessionId, string name, int pNodeId,int fsId);

        void RmoveNode(string sessionId, int nodeId);

        void RenameNode(string sessionId, int childId, string name);

        void MoveNode(string sessionId, int parentId, int nodeId);

        #endregion
    }
}
