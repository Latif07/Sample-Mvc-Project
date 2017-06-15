using System.Collections.Generic;
using Models;

namespace SampleWebProject.Infrastructure.Interface {
    public interface IRoleRepository : IEntityRepository<Role, int> {
        IList<Role> GetRoles();
    }
}