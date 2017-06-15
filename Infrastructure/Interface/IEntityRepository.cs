using System;
using System.Linq;
using System.Linq.Expressions;

namespace SampleWebProject.Infrastructure.Interface {
    public interface IEntityRepository<T, TKey> : IRepository where T : class {
        T GetById(TKey id);
        T GetById(TKey id, params Expression<Func<T, object>>[] includes);
        T GetById(TKey id, Expression<Func<T,bool>> expression,  params Expression<Func<T, object>>[] includes);
        bool Delete(TKey id);
        void PrepareSave(T entity);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
    }
}
