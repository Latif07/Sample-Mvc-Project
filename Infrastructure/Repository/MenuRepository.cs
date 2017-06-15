using System.Collections.Generic;
using System.Linq;
using Models;
using SampleWebProject.Infrastructure.Interface;

namespace SampleWebProject.Infrastructure.Repository {

    public class MenuRepository : BaseRepository<Menu, int>, IMenuRepository {
        public MenuRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork) {
        }

        public IList<Menu> GetMenus() {
            var menus = (from m in Context.Menus select m);
            return menus.ToList();
        }

        public override Menu GetById(int id) {
            return Context.Menus
                .FirstOrDefault(x => x.Id == id);
        }
    }
}