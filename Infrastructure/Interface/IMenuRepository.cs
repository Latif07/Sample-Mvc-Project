using System.Collections.Generic;
using Models;

namespace SampleWebProject.Infrastructure.Interface {

    public interface IMenuRepository : IEntityRepository<Menu, int> {
        IList<Menu> GetMenus();
    }
}