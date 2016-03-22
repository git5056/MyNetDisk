using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskDomain
{
    public class NodeException : Exception
    {
        public NodeException(string message)
            : base(message)
        {

        }

        public NodeException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }


}
