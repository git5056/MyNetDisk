using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;

namespace NetDiskRepository
{
    public interface INodeTreeRepository : IRepository<Node>
    {
        #region Extension Methods

        IList<Node> FilterByPostfix(UserZero uz, string postfix);

        IList<Node> FilterByContentType(UserZero uz, string [] contentType);

        #endregion
    }
}
