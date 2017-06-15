using System.Collections.Generic;
using Models;

namespace SampleWebProject.Infrastructure.Interface {
    public interface IUserService : IBusinessService<User, int> {
        IList<User> GetUsers();
    }
}