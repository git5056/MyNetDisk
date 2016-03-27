using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace NetDiskWeb
{
    public class APrincipal : IPrincipal
    {

        private IIdentity _identity;//用户标识  
        private ArrayList _permissionList;//权限列表  

        /// <summary>  
        /// 返回用户权限列表  
        /// </summary>  
        public ArrayList PermissionList
        {
            get { return _permissionList; }
        }

        /// <summary>  
        /// 获取当前用户标识  
        /// </summary>  
        public IIdentity Identity
        {
            get { return _identity; }  
        }

        /// <summary>  
        /// 当前用户是否指定角色（采用权限值方式，此处返回false）  
        /// </summary>  
        /// <param name="role"></param>  
        /// <returns></returns>  
        public bool IsInRole(string role)
        {
            return false;
        }

        public APrincipal(string userName)
        {
            _identity = new AUser(userName);
            //以下权限根据UserName获取数据库用户拥有的权限值，此次省略  
            _permissionList = new ArrayList();
            _permissionList.Add(1);
            _permissionList.Add(2);
            _permissionList.Add(3);
            _permissionList.Add(4);
            _permissionList.Add(5); 
        }

        /// <summary>  
        /// 判断用户是否拥有某权限  
        /// </summary>  
        /// <param name="permissionid"></param>  
        /// <returns></returns>  

        public bool IsPermissionID(int permissionid)
        {
            return _permissionList.Contains(permissionid);
        }  

    }
}