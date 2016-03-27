using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;
using NetDiskRepository;

namespace NetDiskService
{
    class RoleToUserService : Service<RoleToUser>, IRoleToUserService
    {
        private IRoleToUserRepository RoleToUserRepository
        {
            get
            {
                return _repository as IRoleToUserRepository;
            }
        }
        public string[] GetRolesByUserId(int id)
        {
            return RoleToUserRepository.GetRolesByUserId(id);
        }
    }
}
