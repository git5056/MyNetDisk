using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskDomain
{
    public interface IFileDownloader
    {
        DownloadRecond DownFile(Node node, Func<string, bool> doDown);

        //控制权限
        bool TryDown(object context);
    }
}
