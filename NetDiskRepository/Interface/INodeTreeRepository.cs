using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;

namespace NetDiskRepository
{
    public interface INodeTreeRepository : IRepository<NodeTree>
    {
        #region Extension Methods

        IList<NodeTree> FilterByPostfix(UserZero uz, string postfix);

        IList<NodeTree> FilterByContentType(UserZero uz, string contentType);

        #endregion
    }
}
