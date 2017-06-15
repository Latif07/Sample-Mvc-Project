using System;
using System.Linq;
using System.Linq.Expressions;

namespace SampleWebProject.Infrastructure.Interface {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type of DB Entity</typeparam>
    /// <typeparam name="TKey">Type of Identity Column data tyupe</typeparam>
    public interface IBusinessService<T, in TKey> where T : class, new() {
        T GetById(TKey id);
        T GetById(TKey id, params Expression<Func<T, object>>[] includes);
        T GetById(TKey id,Expression<Func<T,bool>> expression , params Expression<Func<T, object>>[] includes);
        bool Delete(TKey id);
        bool Save(T entity);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        ValidationResult Validate(T entity);
        ValidationResult ValidationResult { get;}
    }

}
