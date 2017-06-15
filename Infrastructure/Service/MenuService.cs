using System.Collections.Generic;
using Models;
using SampleWebProject.Infrastructure.Interface;

namespace SampleWebProject.Infrastructure.Service {
    public class MenuService : BaseService<IMenuRepository, IMenuValidator, Menu, int>, IMenuService
    {
        public MenuService(IMenuRepository repository, IMenuValidator validator, IUnitOfWork unitOfWork) : base(repository, validator, unitOfWork)
        {
        }

        public IList<Menu> GetMenus()
        {
            return Repository.GetMenus();
        }
    }
}