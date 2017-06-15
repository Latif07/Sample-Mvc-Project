using System.Collections.Generic;
using System.Linq;
using Models;
using SampleWebProject.Models;

namespace SampleWebProject.Infrastructure.Interface {
    public interface ICommonRepository {
        IQueryable<UserModel> GetUsers(int dataCount = Constants.MaxRecordCount);
        IList<Menu> GetMenus(string userName);
        IList<Menu> GetMenuItems();
    }
}
