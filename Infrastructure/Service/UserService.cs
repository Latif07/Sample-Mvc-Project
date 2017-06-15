using System.Collections.Generic;
using System.Security;
using Models;
using SampleWebProject.Infrastructure.Interface;
using SecurityManager = SampleWebProject.Infrastructure.Security.SecurityManager;

namespace SampleWebProject.Infrastructure.Service {
    public class UserService : BaseService<IUserRepository, IUserValidator, User, int>, IUserService {
        public UserService(IUserRepository repository, IUserValidator validator, IUnitOfWork unitOfWork)
            : base(repository, validator, unitOfWork) {
        }

        public IList<User> GetUsers() {
            return Repository.GetUsers();
        }
        public override bool Save(User entity) {
            if (ValidationResult.Errors.Count > 0) return false;
            if (entity.Id == Constants.NewItemId) {
                return SecurityManager.CreateUser(entity.Username, entity.Password, 1, entity.ExpireDate, entity.TokenValidity);
            }
            else {
                Repository.PrepareSave(entity);
                return UnitOfWork.SaveChanges()> 0;
            }
        }

    }
}