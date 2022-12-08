using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Core.Services
{
    public class UsersToCategoryService : IUsersToCategoryService
    {

        private readonly IRepository _repo;
        private readonly IDataFunctions _dataFunctions;
        private readonly ILogger<UsersToCategoryService> _logger;

        public UsersToCategoryService(IRepository repo,
            IDataFunctions dataFunctions,
            ILogger<UsersToCategoryService> logger)
        {
            _repo = repo;
            _dataFunctions = dataFunctions;
            _logger = logger;
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            var allUsers = await (from user in _repo.AllReadonly<ApplicationUser>()
                                  select new UserModel()
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      Email = user.Email
                                  }).ToListAsync();
            return allUsers;
        }

        public async Task<List<UserModel>> GetSavedSelectedUsersForCategory(int categoryId)
        {
            var savedSelectedUsersForCategory = await (from userCat in _repo.All<UserCategory>()
                                                       where userCat.CategoryId == categoryId
                                                       where userCat.Category.IsActive == true
                                                       select new UserModel()
                                                       {
                                                           Id = userCat.UserId
                                                       }).ToListAsync();
            return savedSelectedUsersForCategory;
        }

        public async Task<List<UserCategory>> GetUsersSelectedForCategoryToAdd(UserCategoryListModel userCategoryListModel)
        {
            var usersForCategoryToAdd = (from userCat in userCategoryListModel.UsersSelected
                                         select new UserCategory
                                         {
                                             CategoryId = userCategoryListModel.CategoryId,
                                             UserId = userCat.Id
                                         }).ToList();

            return await Task.FromResult(usersForCategoryToAdd);
        }

        public async Task<List<UserCategory>> GetUsersSelectedForCategoryToDelete(int categoryId)
        {
            var usersForCategoryToDelete = await(from userCat in _repo.All<UserCategory>()
                                                 where userCat.CategoryId == categoryId
                                                 where userCat.Category.IsActive == true
                                                 select new UserCategory()
                                                 {
                                                     Id = userCat.Id,
                                                     CategoryId = categoryId,
                                                     UserId = userCat.UserId
                                                 }).ToListAsync();
            return usersForCategoryToDelete;
        }

        public async Task UpdateUsersSubscriptionsForCategory(UserCategoryListModel userCategoryListModel)
        {
            List<UserCategory>? usersToAdd = null;

            if (userCategoryListModel.UsersSelected != null)
            {
                usersToAdd = await GetUsersSelectedForCategoryToAdd(userCategoryListModel);
            }

            var usersToDelete = await GetUsersSelectedForCategoryToDelete(userCategoryListModel.CategoryId);

#pragma warning disable CS8604 // Possible null reference argument.
			await _dataFunctions.UpdateUserCategoryEntityAsync(usersToDelete, usersToAdd);
#pragma warning restore CS8604 // Possible null reference argument.
		}
    }
}
