using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;
using NetDiskRepository;

namespace NetDiskService
{
    public class NodeTreeService : Service<Node>, INodeTreeService
    {

        #region Property
        private INodeTreeRepository NodeTreeRepository
        {
            get
            {
                return _repository as INodeTreeRepository;
            }
        }

        public virtual ISessionService SessionService
        {
            get;
            set;
        }

        #endregion

        public IList<Node> FilterByContentType(string sessionId, string contentType)
        {
            var current=SessionService.GetCurrentUser(sessionId);
            if (current is UserZero)
            {
                return NodeTreeRepository.FilterByContentType(current as UserZero,contentType);
            }
            throw new Exception("visitor denied");
        }

        public IList<Node> FilterByPostfix(string sessionId, string postfix)
        {
            var current = SessionService.GetCurrentUser(sessionId);
            if (current is UserZero)
            {
                return NodeTreeRepository.FilterByPostfix(current as UserZero, postfix);
            }
            throw new Exception("visitor denied");
        }
    }
}
