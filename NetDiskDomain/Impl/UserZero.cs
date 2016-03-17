using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskDomain.Impl
{
    class UserZero : UserBase,IFileManager
    {
        public void Upfile()
        {
            throw new NotImplementedException();
        }

        public void Downfile()
        {
            throw new NotImplementedException();
        }
    }
}
