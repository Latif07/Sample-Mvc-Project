using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Models;
using SampleWebProject.Infrastructure.Interface;

namespace SampleWebProject.Infrastructure.Repository {
    public abstract class BaseRepository<TEntity, TKey> where TEntity : class, new() {
        private readonly IUnitOfWork _unitOfWork;
        private DbSet<TEntity> DbSet { get { return Context.Set<TEntity>(); } }
        private IQueryable<TEntity> DbSetQuery { get { return DbSet.AsQueryable(); } }
        public BaseRepository(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public SampleEntities Context {
            get {
                return _unitOfWork.Context;
            }
        }

        public virtual TEntity GetById(TKey id)
        {
            return DbSet.Where("Id == @0", id).FirstOrDefault();
        }

        public virtual TEntity GetById(TKey id, params Expression<Func<TEntity, object>>[] includes) {
            var dbSet = DbSetQuery;
            foreach (var include in includes) dbSet = dbSet.Include(include);
            return dbSet.Where("Id == @0", id).FirstOrDefault();
        }

        public virtual TEntity GetById(TKey id, Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes) {
            var dbSet = DbSetQuery;
            foreach (var include in includes) dbSet = dbSet.Include(include);
            dbSet = dbSet.Where(expression);
            return dbSet.Where("Id == @0", id).FirstOrDefault();
        }

        public virtual bool Delete(TKey id) {
            bool result = false;
            var authorization = GetById(id);
            if (authorization != null) {
                DbSet.Remove(authorization);
                _unitOfWork.SaveChanges();
                result = true;
            }

            return result;
        }

        public virtual void PrepareSave(TEntity entity) {
            var dynamicEntity = (dynamic)entity;
            if (dynamicEntity.Id != 0)
                Context.Entry(entity).State = EntityState.Modified;
            else
                DbSet.Add(dynamicEntity);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSetQuery;
        }

        public virtual IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            var dbSet = DbSetQuery;
            foreach (var include in includes) dbSet = dbSet.Include(include);
            return dbSet;
        }
    }
}