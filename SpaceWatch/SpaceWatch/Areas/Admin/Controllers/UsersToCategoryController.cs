using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceWatch.Areas.Admin.Models;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceWatch.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersToCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataFunctions _dataFunctions;
        private readonly IRepository _repo;
        public UsersToCategoryController(ApplicationDbContext context, 
            IDataFunctions dataFunctions,
            IRepository repository)
        {
            _context = context;
            _dataFunctions = dataFunctions;
            _repo = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersForCategory(int categoryId)
        {
            UserCategoryListModel userCategoryListModel = new UserCategoryListModel();

            var allUsers = await GetAllUsers();

            var selectedUsersForCategory = await GetSavedSelectedUsersForCategory(categoryId);

            userCategoryListModel.Users = allUsers;
            userCategoryListModel.UsersSelected = selectedUsersForCategory;

            return PartialView("_UsersListViewPartial", userCategoryListModel);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSelectedUsers([Bind(
            "CategoryId, UsersSelected")] UserCategoryListModel userCategoryListModel)
        {
            List<UserCategory> usersSelectedForCategoryToAdd = null;
            if (userCategoryListModel.UsersSelected != null)
            {
                usersSelectedForCategoryToAdd = await
                    GetUsersSelectedForCategoryToAdd(userCategoryListModel);
            }
            var usersSelectedForCategoryToDelete = await
                GetUsersSelectedForCategoryToDelete(userCategoryListModel.CategoryId);

            await _dataFunctions.UpdateUserCategoryEntityAsync(
                usersSelectedForCategoryToDelete, usersSelectedForCategoryToAdd);
            System.Threading.Thread.Sleep(500);

            userCategoryListModel.Users = await GetAllUsers();

            return PartialView("_UsersListViewPartial", userCategoryListModel);
        }

        private async Task<List<UserModel>> GetAllUsers()
        {
            var allUsers = await (from user in _context.Users
                                  select new UserModel
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      Email=user.Email
                                  }).ToListAsync();
            return allUsers;
        }


        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            UserCategoryListModel userCategoryListModel = new UserCategoryListModel();

            var allUsers = await GetAllUsers();

           

            userCategoryListModel.Users = allUsers;
            userCategoryListModel.UsersSelected = null;

            return View("ManageUsers", userCategoryListModel);
        }


        private async Task<List<UserCategory>> GetUsersSelectedForCategoryToAdd(UserCategoryListModel userCategoryListModel)
        {
            var usersForCategoryToAdd = (from userCat in userCategoryListModel.UsersSelected
                                         select new UserCategory
                                         {
                                             CategoryId = userCategoryListModel.CategoryId,
                                             UserId = userCat.Id
                                         }).ToList();
            return await Task.FromResult(usersForCategoryToAdd);
        }

        private async Task<List<UserCategory>> GetUsersSelectedForCategoryToDelete(int categoryId) 
        {
            var usersForCategoryToDelete = await (from userCat in _context.UserCategories
                                                  where userCat.CategoryId == categoryId
                                                  select new UserCategory
                                                  {
                                                      Id = userCat.Id,
                                                      CategoryId = categoryId,
                                                      UserId = userCat.UserId
                                                  }).ToListAsync();
            return usersForCategoryToDelete;
        }

        private async Task<List<UserModel>>GetSavedSelectedUsersForCategory(int categoryId)
        {
            var savedSelectedUsersForCategory = await (from userCat in _context.UserCategories
                                                       where userCat.CategoryId == categoryId
                                                       select new UserModel
                                                       {
                                                           Id = userCat.UserId
                                                       }).ToListAsync();
            return savedSelectedUsersForCategory;
        }
    }
}
