
using System;
using System.Linq;
using System.Linq.Expressions;
using SampleWebProject.Infrastructure.Interface;

namespace SampleWebProject.Infrastructure.Service {
    public class BaseService<TRepository, TValidator, TEntity, TKey>
        where TRepository : IEntityRepository<TEntity, TKey>
        where TEntity : class
        where TValidator : IBusinessValidator<TEntity> {
        private readonly TRepository _repository;
        private readonly TValidator _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ValidationResult _validationResult;

        private BaseService() {
            _validationResult = new ValidationResult();
            _unitOfWork = null;
            _repository = default(TRepository);
            _validator = default(TValidator);
        }
        public BaseService(TRepository repository, TValidator validator, IUnitOfWork unitOfWork) {
            _repository = repository;
            _validator = validator;
            _unitOfWork = unitOfWork;
            _validationResult = new ValidationResult();
        }
        public virtual TEntity Get(TKey id) {
            return _repository.GetById(id);
        }
        public virtual TEntity GetById(TKey id) {
            return _repository.GetById(id);
        }

        public virtual TEntity GetById(TKey id, params Expression<Func<TEntity, object>>[] includes) {
            return _repository.GetById(id, includes);
        }

        public virtual TEntity GetById(TKey id, Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes) {
            return _repository.GetById(id, expression, includes);
        }

        public virtual bool Delete(TKey id) {
            return _repository.Delete(id);
        }

        public virtual bool Save(TEntity entity) {
            Validate(entity);
            if (_validationResult.Errors.Count > 0) return false;
            _repository.PrepareSave(entity);
            return _unitOfWork.SaveChanges() > 0;
        }

        public virtual IQueryable<TEntity> GetAll() {
            return _repository.GetAll();
        }

        public virtual IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes) {
            return _repository.GetAll(includes);
        }

        public ValidationResult Validate(TEntity entity) {
            _validationResult.Set(_validator.Validate(entity));
            return _validationResult;
        }

        public ValidationResult ValidationResult { get { return _validationResult; } }
        public TRepository Repository { get { return _repository; } }
        public IUnitOfWork UnitOfWork { get { return _unitOfWork; } }
    }
}