using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskDomain
{
    public class Vistor : AbstractUserRunTime, IFileDownload
    {
        public Vistor(Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            this._Session = session;
        }

        private Session _session;

        public override Session _Session
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
            }
        }

        /// <summary>
        /// return -1
        /// </summary>
        /// <returns></returns>
        protected override int GetUserId()
        {
            return -1;
        }

        public void Downfile()
        {
            throw new NotImplementedException();
        }
    }

}
