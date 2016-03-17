using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskRepository
{
    public interface IRepository<T>
    {
        void Save(T entity);

        void Update(T entity);

        void SaveOrUpdate(T entity);

        void Delete(object entity);

        T FindById(object id);

        //T LoadChild(T entity);

        IList<T> FindByHQL(string hql, params object[] param);
    }
}
