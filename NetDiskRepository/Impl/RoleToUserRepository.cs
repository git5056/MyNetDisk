using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;

namespace NetDiskRepository
{
    class RoleToUserRepository : Repository<RoleToUser>, IRoleToUserRepository
    {
        public virtual IRoleRepository RoleRepository
        {
            get;
            set;
        }

        public string[] GetRolesByUserId(int id)
        {
            var list = FindByHQL("from " + typeof(RoleToUser) + " where userId=?", id);
            var query = from mroles in list select mroles.role.GetAllRole() into arole from role in arole select role;
            return query.Distinct().ToArray();
        }
    }
}
