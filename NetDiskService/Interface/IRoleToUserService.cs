using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;

namespace NetDiskService
{
    public interface IRoleToUserService : IService<RoleToUser>
    {
        string[] GetRolesByUserId(int id);
    }

}
