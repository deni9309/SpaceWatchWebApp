using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceWatch.Core.Contracts.DefaultArea;
using SpaceWatch.Core.Models.DefaultArea;
using SpaceWatch.Infrastructure.Data;
using System.Threading.Tasks;

namespace SpaceWatch.Controllers
{
    [Authorize]
    public class CategoriesToUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataFunctions _dataFunctions;
        private readonly ICategoriesToUserService _categoriesToUserService;

        public CategoriesToUserController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IDataFunctions dataFunctions,
            ICategoriesToUserService categoriesToUserService)
        {
            _context = context;
            _userManager = userManager;
            _dataFunctions = dataFunctions;
            _categoriesToUserService = categoriesToUserService;
        }

        public async Task<IActionResult> Index()
        {
            CategoriesForUserModel categoriesForUser = new CategoriesForUserModel();
			var userId = _userManager.GetUserAsync(User).Result?.Id;

            categoriesForUser.UserId = userId;

            categoriesForUser.Categories = await _categoriesToUserService
                .GetCategoriesThatHaveContent();

            categoriesForUser.CategoriesSelected = await _categoriesToUserService
                .GetCategoriesCurrentlySavedForUser(userId);

            return View(categoriesForUser);

			//CategoriesToUserModel categoriesToUserModel = new CategoriesToUserModel();

			//var userId = _userManager.GetUserAsync(User).Result?.Id;

			//categoriesToUserModel.Categories = await GetCategoriesThatHaveContent();
			//categoriesToUserModel.CategoriesSelected = await GetCategoriesCurrentlySavedForUser(userId);
			//categoriesToUserModel.UserId = userId;

			//return View(categoriesToUserModel);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string[] categoriesSelected)
        {
			var userId = _userManager.GetUserAsync(User).Result?.Id;

            await _dataFunctions.UpdateUserCategoryEntityAsync(
                await _categoriesToUserService.GetCategoriesToDeleteForUser(userId),
                await _categoriesToUserService.GetCategoriesToAddForUser(categoriesSelected, userId)
               );

			//var userId = _userManager.GetUserAsync(User).Result?.Id;

			//List<UserCategory> userCategoriesToDelete = await GetCategoriesToDeleteForUser(userId);
			//List<UserCategory> userCategoriesToAdd = GetCategoriesToAddForUser(categoriesSelected, userId);

			//await _dataFunctions.UpdateUserCategoryEntityAsync(userCategoriesToDelete, userCategoriesToAdd);
			
            return RedirectToAction("Index", "Home");
		}

        //private async Task<List<Category>> GetCategoriesThatHaveContent()
        //{
        //    var categoriesThatHaveContent = await (from category in _context.Categories
        //                                           join categoryItem in _context.CategoryItems
        //                                           on category.Id equals categoryItem.CategoryId
        //                                           join content in _context.Content
        //                                           on categoryItem.Id equals content.CategoryItem.Id
        //                                           where categoryItem.IsActive== true
        //                                           where content.IsActive == true
        //                                           select new Category
        //                                           {
        //                                               Id = category.Id,
        //                                               Title = category.Title,
        //                                               Description = category.Description,
        //                                               ThumbnailImagePath = category.ThumbnailImagePath
        //                                           }).Distinct().ToListAsync();
        //    return categoriesThatHaveContent;
        //}

        //private async Task<List<Category>> GetCategoriesCurrentlySavedForUser(string userId)
        //{
        //    var categoriesCurrentlySavedForUser = await (from userCategory in _context.UserCategories
        //                                                 join cat in _context.Categories
        //                                                 on userCategory.CategoryId equals cat.Id
        //                                                 join catItem in _context.CategoryItems
        //                                                 on cat.Id equals catItem.CategoryId
        //                                                 where catItem.IsActive== true
        //                                                 where userCategory.UserId == userId
        //                                                 where userCategory.Category.IsActive == true
        //                                                 select new Category
        //                                                 {
        //                                                     Id = userCategory.CategoryId
        //                                                 }).ToListAsync();
        //    return categoriesCurrentlySavedForUser;
        //}

        //private async Task<List<UserCategory>> GetCategoriesToDeleteForUser(string userId)
        //{
        //    var categoriesToDelete = await (from userCat in _context.UserCategories
        //                                           where userCat.UserId == userId
        //                                           select new UserCategory
        //                                           {
        //                                               Id = userCat.Id,
        //                                               CategoryId = userCat.CategoryId,
        //                                               UserId = userCat.UserId
        //                                           }).ToListAsync();

        //    return categoriesToDelete;
        //}

        //private List<UserCategory> GetCategoriesToAddForUser(string[] categoriesSelected, string userId)
        //{
        //    var categoriesToAdd = (from categoryId in categoriesSelected
        //                           select new UserCategory
        //                           {
        //                               UserId = userId,
        //                               CategoryId = int.Parse(categoryId)
        //                           }).ToList();
        //    return categoriesToAdd;
        //}    
    }
}
