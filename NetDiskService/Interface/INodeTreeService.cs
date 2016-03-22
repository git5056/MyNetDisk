using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;

namespace NetDiskService
{
    public interface INodeTreeService : IService<Node>
    {
        #region Extension Methods

        IList<Node> FilterByPostfix(string sessionId, string postfix);
        IList<Node> FilterByContentType(string sessionId, string contentType);

        #endregion
    }
}
