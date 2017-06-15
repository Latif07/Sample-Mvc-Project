using System.Collections.Generic;
using System.Linq;
using Models;
using SampleWebProject.Models;

namespace SampleWebProject.Infrastructure.Interface
{
    public interface ICommonService
    {
        IQueryable<UserModel> GetUsers(int dataCount = Constants.MaxRecordCount, bool createWithAllItem = true);
        IList<Menu> GetMenus(string userName);
        IList<Menu> GetMenuItems();
    }
}
