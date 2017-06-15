using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using SampleWebProject.Infrastructure.Interface;
using SampleWebProject.Models;

namespace SampleWebProject.Infrastructure.Repository {
    public class CommonRepository : ICommonRepository {
        private readonly IUnitOfWork _unitOfWork;

        public CommonRepository(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public SampleEntities Context {
            get {
                return _unitOfWork.Context;
            }
        }

       public IQueryable<UserModel> GetUsers(int dataCount = Constants.MaxRecordCount) {
            var users = Context.Users.Select(x => new UserModel { Id = x.Id, Name = x.Username })
                               .OrderBy(x => x.Name);

            var result = dataCount > 0 ? users.Take<UserModel>(dataCount) : users;

            return result;
        }

        public IList<Menu> GetMenus(string userName) {
            using (var context = new SampleEntities()) {
                var user = context.Users.FirstOrDefault(x => x.Username == userName);
                if (user == null) return null;
                IQueryable<Menu> menus;
                if (user.IsAdmin)
                    menus = (from m in context.Menus select m);
                else {
                    var userMenus = (from m in context.Menus
                        join um in context.UserMenus on m.Id equals um.MenuId
                        join u in context.Users on um.UserId equals u.Id
                        where u.Id == user.Id
                        select m);
                    var roleMenus = (from u in context.Users
                        join ur in context.UserRoles on u.Id equals ur.UserId
                        join rm in context.RoleMenus on ur.RoleId equals rm.RoleId
                        join m in context.Menus on rm.MenuId equals m.Id
                        where u.Id == user.Id
                        select m);
                    menus = userMenus.Union(roleMenus);
                }
                return menus.ToList();
            }
        }

        public IList<Menu> GetMenuItems() {
            using (var context = new SampleEntities()) {
                var menus = (from m in context.Menus select m);
                return menus.ToList();
            }
        }
    }
}