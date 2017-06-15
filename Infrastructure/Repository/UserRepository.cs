using System.Collections.Generic;
using System.Linq;
using Models;
using SampleWebProject.Infrastructure.Interface;

namespace SampleWebProject.Infrastructure.Repository {

    public class UserRepository : BaseRepository<User, int>, IUserRepository {
        public UserRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork) {
        }

        public IList<User> GetUsers() {
            var users = (from u in Context.Users
                         select u);
            return users.ToList();
        }

        public override User GetById(int id) {
            return Context.Users
                .FirstOrDefault(x => x.Id == id);
        }
    }
}