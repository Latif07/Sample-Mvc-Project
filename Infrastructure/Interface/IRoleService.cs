using System.Collections.Generic;
using Models;

namespace SampleWebProject.Infrastructure.Interface {

    public interface IRoleService : IBusinessService<Role, int> {
        IList<Role> GetRoles();
    }
}