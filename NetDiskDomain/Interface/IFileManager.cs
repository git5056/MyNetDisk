using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskDomain
{
    interface IFileManager:IFileUploader,IFileDownloader,IFileShare
    {

    }
}
