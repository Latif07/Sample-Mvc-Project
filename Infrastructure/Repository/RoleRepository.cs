using System.Collections.Generic;
using System.Linq;
using Models;
using SampleWebProject.Infrastructure.Interface;

namespace SampleWebProject.Infrastructure.Repository {

    public class RoleRepository : BaseRepository<Role, int>, IRoleRepository {
        public RoleRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork) {
        }

        public IList<Role> GetRoles() {
            var roles = (from r in Context.Roles
                         join rm in Context.RoleMenus on r.Id equals rm.RoleId into l1
                         from rm in l1.DefaultIfEmpty()
                         join m in Context.Menus on rm.MenuId equals m.Id into l2
                         from m in l2.DefaultIfEmpty()
                         join ur in Context.UserRoles on r.Id equals ur.RoleId into l3
                         from ur in l3.DefaultIfEmpty()
                         join u in Context.Users on ur.UserId equals u.Id into l4
                         from u in l4.DefaultIfEmpty()
                         select  r);
            return roles.ToList();
        }

        public override Role GetById(int id) {
           return (from r in Context.Roles
                    join rm in Context.RoleMenus on r.Id equals rm.RoleId into l1
                    from rm in l1.DefaultIfEmpty()
                    join m in Context.Menus on rm.MenuId equals m.Id into l2
                    from m in l2.DefaultIfEmpty()
                    join ur in Context.UserRoles on r.Id equals ur.RoleId into l3
                    from ur in l3.DefaultIfEmpty()
                    join u in Context.Users on ur.UserId equals u.Id into l4
                    from u in l4.DefaultIfEmpty()
                    where r.Id == id
                    select r).FirstOrDefault();
        }
    }
}