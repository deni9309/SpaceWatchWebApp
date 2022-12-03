using SpaceWatch.Core.Models;
using SpaceWatch.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWatch.Core.Contracts
{
    public interface IUsersToCategoryService
    {

        Task<List<UserModel>> GetAllUsers();

        Task<List<UserCategory>> GetUsersSelectedForCategoryToAdd(UserCategoryListModel userCategoryListModel);

        Task<List<UserCategory>> GetUsersSelectedForCategoryToDelete(int categoryId);

        Task UpdateUsersSubscriptionsForCategory(UserCategoryListModel userCategoryListModel);

        Task<List<UserModel>> GetSavedSelectedUsersForCategory(int categoryId);
    }
}
