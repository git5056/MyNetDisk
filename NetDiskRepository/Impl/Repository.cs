using Spring.Data.NHibernate.Generic.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskRepository
{
    public class Repository<T> : HibernateDaoSupport, IRepository<T>
    {
        public void Save(T entity)
        {
            this.HibernateTemplate.Save(entity);
        }

        public void Update(T entity)
        {
            this.HibernateTemplate.Update(entity);
        }

        public void SaveOrUpdate(T entity)
        {
            this.HibernateTemplate.SaveOrUpdate(entity);
        }

        public void Delete(object entity)
        {
            this.HibernateTemplate.Delete(entity);
        }

        public T FindById(object id)
        {
            return this.HibernateTemplate.Get<T>(id);
            //return this.Session.Get<T>(id);
        }

        /// <summary>
        /// bug,,,
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IList<T> FindByHQL(string hql, params object[] args)
        {
            using (var session = this.Session)
            {
                
                var query = session.CreateQuery(hql);
                for (int i = 0; args != null && i < args.Length; i++)
                {
                    query.SetParameter(i, args);
                }
                return query.List<T>();
            }
        }

    }
}
