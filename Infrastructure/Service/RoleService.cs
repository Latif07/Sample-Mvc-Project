using System.Collections.Generic;
using Models;
using SampleWebProject.Infrastructure.Interface;

namespace SampleWebProject.Infrastructure.Service {

    public class RoleService : BaseService<IRoleRepository, IRoleValidator, Role, int>, IRoleService {
        public RoleService(IRoleRepository repository, IRoleValidator validator, IUnitOfWork unitOfWork)
            : base(repository, validator, unitOfWork) {
        }

        public IList<Role> GetRoles() {
            return Repository.GetRoles();
        }
    }
}