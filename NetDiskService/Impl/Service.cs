using NetDiskRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskService
{
    public class Service<T> : IService<T>, IRepositoryAware<T>
    {

        protected IRepository<T> _repository;
        public void Save(T entity)
        {
            _repository.Save(entity);
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
        }

        public void SaveOrUpdate(T entity)
        {
            _repository.SaveOrUpdate(entity);
        }

        public void Delete(object entity)
        {
            _repository.Delete(entity);
        }

        public T FindById(object id)
        {
            return _repository.FindById(id);
        }

        public IList<T> FindByHQL(string hql, params object[] param)
        {
            return _repository.FindByHQL(hql, param);
        }

        public IRepository<T> Repository
        {
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException("Repository");
                }
                _repository = value;
            }
        }

    }
}
