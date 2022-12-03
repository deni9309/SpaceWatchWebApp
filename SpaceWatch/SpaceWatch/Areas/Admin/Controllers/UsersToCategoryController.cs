using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using System.Threading.Tasks;

namespace SpaceWatch.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersToCategoryController : Controller
    {
        //private readonly ApplicationDbContext _context;     
       
        private readonly ICategoryService _categoryService;
        private readonly IUsersToCategoryService _usersToCategoryService;
        public UsersToCategoryController(IUsersToCategoryService usersToCategoryService,
            ICategoryService categoryService)
        {
            _usersToCategoryService = usersToCategoryService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersForCategory(int categoryId)
        {
            UserCategoryListModel userCategoryListModel = new UserCategoryListModel();

            var allUsers = await _usersToCategoryService.GetAllUsers();

            var selectedUsersForCategory = await _usersToCategoryService.GetSavedSelectedUsersForCategory(categoryId);

            userCategoryListModel.Users = allUsers;
            userCategoryListModel.UsersSelected = selectedUsersForCategory;

            return PartialView("_UsersListViewPartial", userCategoryListModel);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSelectedUsers([Bind("CategoryId, UsersSelected")] UserCategoryListModel userCategoryListModel)
        {
            await _usersToCategoryService.UpdateUsersSubscriptionsForCategory(userCategoryListModel);

            System.Threading.Thread.Sleep(500);

            userCategoryListModel.Users = await _usersToCategoryService.GetAllUsers();

            return PartialView("_UsersListViewPartial", userCategoryListModel);
            //List<UserCategory> usersSelectedForCategoryToAdd = null;
            //if (userCategoryListModel.UsersSelected != null)
            //{
            //    usersSelectedForCategoryToAdd = await
            //        GetUsersSelectedForCategoryToAdd(userCategoryListModel);
            //}
            //var usersSelectedForCategoryToDelete = await
            //    GetUsersSelectedForCategoryToDelete(userCategoryListModel.CategoryId);

            //await _dataFunctions.UpdateUserCategoryEntityAsync(
            //    usersSelectedForCategoryToDelete, usersSelectedForCategoryToAdd);
            //System.Threading.Thread.Sleep(500);

            //userCategoryListModel.Users = await GetAllUsers();

            //return PartialView("_UsersListViewPartial", userCategoryListModel);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            UserCategoryListModel userCategoryListModel = new UserCategoryListModel();

            var allUsers = await _usersToCategoryService.GetAllUsers();

            userCategoryListModel.Users = allUsers;
            userCategoryListModel.UsersSelected = null;

            return View("ManageUsers", userCategoryListModel);
        }

        //private async Task<List<UserModel>> GetAllUsers()
        //{
        //    var allUsers = await (from user in _context.Users
        //                          select new UserModel
        //                          {
        //                              Id = user.Id,
        //                              UserName = user.UserName,
        //                              FirstName = user.FirstName,
        //                              LastName = user.LastName,
        //                              Email=user.Email
        //                          }).ToListAsync();
        //    return allUsers;
        //}

        //private async Task<List<UserCategory>> GetUsersSelectedForCategoryToAdd(UserCategoryListModel userCategoryListModel)
        //{
        //    var usersForCategoryToAdd = (from userCat in userCategoryListModel.UsersSelected
        //                                 select new UserCategory
        //                                 {
        //                                     CategoryId = userCategoryListModel.CategoryId,
        //                                     UserId = userCat.Id
        //                                 }).ToList();
        //    return await Task.FromResult(usersForCategoryToAdd);
        //}

        //private async Task<List<UserCategory>> GetUsersSelectedForCategoryToDelete(int categoryId) 
        //{
        //    var usersForCategoryToDelete = await (from userCat in _context.UserCategories
        //                                          where userCat.CategoryId == categoryId
        //                                          select new UserCategory
        //                                          {
        //                                              Id = userCat.Id,
        //                                              CategoryId = categoryId,
        //                                              UserId = userCat.UserId
        //                                          }).ToListAsync();
        //    return usersForCategoryToDelete;
        //}

        //private async Task<List<UserModel>>GetSavedSelectedUsersForCategory(int categoryId)
        //{
        //    var savedSelectedUsersForCategory = await (from userCat in _context.UserCategories
        //                                               where userCat.CategoryId == categoryId
        //                                               select new UserModel
        //                                               {
        //                                                   Id = userCat.UserId
        //                                               }).ToListAsync();
        //    return savedSelectedUsersForCategory;
        //}
    }
}
