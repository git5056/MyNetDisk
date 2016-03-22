using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;

namespace NetDiskRepository
{
    public class NodeTreeRepository : Repository<Node>, INodeTreeRepository
    {

        public IList<Node> FilterByPostfix(UserZero uz, string postfix)
        {
            var nodes = new List<Node>();
            foreach (Node i in uz.RootNode)
            {
                //i.FileSource != null判断是否是目录,Nhibernate太不好驾驭了，非得NULL才可以， 和我初衷有点相悖啊
                if (i.FileSource != null && !i.IsRemoved() && i.FileSource.postfix == postfix)
                {
                    nodes.Add(i);
                }
            }
            return nodes;
        }

        public IList<Node> FilterByContentType(UserZero uz, string contentType)
        {
            var nodes = new List<Node>();
            foreach (Node i in uz.RootNode)
            {
                //i.FileSource != null判断是否是目录,Nhibernate太不好驾驭了，非得NULL才可以， 和我初衷有点相悖啊
                if (i.FileSource != null && !i.IsRemoved() && i.FileSource.content_type == contentType)
                {
                    nodes.Add(i);
                }
            }
            return nodes;
        }

        
    }
}
