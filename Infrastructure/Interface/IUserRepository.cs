using System.Collections.Generic;
using Models;

namespace SampleWebProject.Infrastructure.Interface {

    public interface IUserRepository : IEntityRepository<User, int> {
        IList<User> GetUsers();
    }
}