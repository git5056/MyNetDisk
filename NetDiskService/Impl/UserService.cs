using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;
using NetDiskRepository;

namespace NetDiskService
{
    public class UserService : Service<UserZero>, IUserService
    {
        private IUserRepository UserRepository
        {
            get
            {
                return _repository as IUserRepository;
            }
        }
        public virtual ISessionService SessionService
        {
            get;
            set;
        }

        public virtual INodeTreeService NodeTreeService
        {
            get;
            set;
        }

        

        public Session Log(string sessionId, string userId, string userPwd)
        {
            var current = SessionService.GetCurrentUser(sessionId);
            
            if (current is Vistor)
            {
                var newSession = UserRepository.Log(current as Vistor, userId, userPwd);
                return newSession;// (_repository as IUserRepository).Log(current as Vistor, userId, userPwd);

            }
            else
            {
                throw new Exception("不能重复登录");
            }
           
        }


        public void DownloadFile(string sessionId, int nodeTreeId, Func<string, bool> doDown)
        {
            var current = SessionService.GetCurrentUser(sessionId);
            var node = NodeTreeService.FindById(nodeTreeId);
            if (node == null)
            {
                throw new Exception("不存在此节点");
            }
            //if(cu)
            
            UserRepository.DownloadFile((IFileDownloader)current, node, doDown);
        }

        public void UploadFile(string sessionId, DoUpHandle doUp)
        {
            if (doUp == null)
            {
                throw new ArgumentNullException("doUp");
            }
            var current = SessionService.GetCurrentUser(sessionId);
            if (current is UserZero)
            {
                UserRepository.UploadFile((current as UserZero), doUp);
                return;
            }
            throw new Exception("visitor denied");
        }
        public void AddNode(string sessionId, string name, int pNodeId,int fsId)
        {
            var current = SessionService.GetCurrentUser(sessionId);
            if (current is UserZero)
            {
                 UserRepository.AddNode((current as UserZero), name, pNodeId, fsId);
                 return;
            }
            throw new Exception("visitor denied");

        }

        public void RmoveNode(string sessionId, int nodeId)
        {
            var current = SessionService.GetCurrentUser(sessionId);
            if (current is UserZero)
            {
                UserRepository.RmoveNode((current as UserZero), nodeId);
                return;
            }
            throw new Exception("visitor denied");
        }


        public void RenameNode(string sessionId, int childId, string name)
        {
            var current = SessionService.GetCurrentUser(sessionId);
            if (current is UserZero)
            {
                UserRepository.RenameNode((current as UserZero), childId, name);
                return;
            }
            throw new Exception("visitor denied");
        }


        public void MoveNode(string sessionId, int parentId, int nodeId)
        {
            var current = SessionService.GetCurrentUser(sessionId);
            if (current is UserZero)
            {
                UserRepository.MoveNode((current as UserZero), parentId, nodeId);
                return;
            }
            throw new Exception("visitor denied");
        }



    }
}
