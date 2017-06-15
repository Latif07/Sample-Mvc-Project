using SampleWebProject.Infrastructure.Interface;
using System.Collections.Generic;
using System.Linq;
using System;
using Models;
using SampleWebProject.Models;

namespace SampleWebProject.Infrastructure.Service {
    public class CommonService : ICommonService {

        private readonly ICommonRepository _commonRepository;
        public CommonService(ICommonRepository commonRepository) {
            _commonRepository = commonRepository;
        }
        
        public IQueryable<UserModel> GetUsers(int dataCount = Constants.MaxRecordCount, bool createWithAllItem = true) {
            var users = _commonRepository.GetUsers(dataCount).ToList();

            if (createWithAllItem)
                users.Insert(0, ViewModelBase.CreateAllItem<UserModel>());

            return users.AsQueryable();
        }

        public IList<Menu> GetMenus(string userName) {
            return _commonRepository.GetMenus(userName);
        }

        public IList<Menu> GetMenuItems () {
            return _commonRepository.GetMenuItems();
        }
       
    }
}