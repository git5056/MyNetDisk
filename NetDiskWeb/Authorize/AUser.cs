using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace NetDiskWeb
{
    public class AUser : IIdentity
    {
        private string _userName;

        public string[] Roles
        {
            get;
            set;
        }

        public AUser(string name, params string[] roles)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            //if (roles == null)
            //{
            //    throw new ArgumentNullException("");
            //}

            this._userName = name;
            this.Roles = roles;
        }


        public string Name
        {
            get { return this._userName; }
        } 

        public string AuthenticationType
        {
            get { return "Form"; }  
        }

        public bool IsAuthenticated
        {
            get { return true; }  
        }

    }
}