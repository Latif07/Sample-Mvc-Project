using System.Collections.Generic;
using Models;

namespace SampleWebProject.Infrastructure.Interface {

    public interface IMenuService : IBusinessService<Menu, int> {
        IList<Menu> GetMenus();
    }
}