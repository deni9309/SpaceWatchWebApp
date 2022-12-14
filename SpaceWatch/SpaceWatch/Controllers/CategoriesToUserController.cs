using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpaceWatch.Core.Contracts.DefaultArea;
using SpaceWatch.Core.Models.DefaultArea;
using SpaceWatch.Infrastructure.Data;
using System.Threading.Tasks;

namespace SpaceWatch.Controllers
{
    [Authorize]
    public class CategoriesToUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataFunctions _dataFunctions;
        private readonly ICategoriesToUserService _categoriesToUserService;

        public CategoriesToUserController(UserManager<ApplicationUser> userManager,
            IDataFunctions dataFunctions,
            ICategoriesToUserService categoriesToUserService)
        {          
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
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string[] categoriesSelected)
        {
			var userId = _userManager.GetUserAsync(User).Result?.Id;

            await _dataFunctions.UpdateUserCategoryEntityAsync(
                await _categoriesToUserService.GetCategoriesToDeleteForUser(userId),
                await _categoriesToUserService.GetCategoriesToAddForUser(categoriesSelected, userId));
			
            return RedirectToAction("Index", "Home");
		}
    }
}
