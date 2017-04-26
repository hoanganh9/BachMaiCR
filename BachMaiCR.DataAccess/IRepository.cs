using System;
using System.Linq;
using System.Linq.Expressions;

namespace BachMaiCR.DataAccess
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        IQueryable<T> Find(Expression<Func<T, bool>> predicate);

        T GetById(int id);

        void Delete(T entity);

        void Add(T entity);

        void Update(T entity);

        int Save();
    }
}