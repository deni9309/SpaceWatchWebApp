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
    }
}
