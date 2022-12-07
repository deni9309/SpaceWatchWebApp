using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts.DefaultArea;
using SpaceWatch.Core.Models.DefaultArea;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceWatch.Controllers
{
    /// <summary>
    /// Provides different layout for user. If user is not sign in to the system he/she sees a dafault home page.
    /// For signed in users, layout is strongly personalised by displaying only categories he/she is assigned for.   
    /// </summary>
    public class HomeController : Controller
    {   
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHomeService _homeService;
        private readonly ICategoriesToUserService _categoriesToUserService;

        public HomeController(ILogger<HomeController> logger,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IHomeService homeService,
            ICategoriesToUserService categoriesToUserService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _homeService = homeService;
            _categoriesToUserService = categoriesToUserService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryItemDetailsModel> itemsDetails = null;
            IEnumerable<GroupedCategoryItemsByCategory> groupedItems = null;

            CategoryDetailsViewModel categoryDetailsModel = new CategoryDetailsViewModel();

            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    var userClaims = User.Claims;
                    var userId = userClaims.ElementAt(0).Value;

                    itemsDetails = await _homeService.GetCategoryItemDetailsForUser(userId);

                    groupedItems = await _homeService.GetGroupedCategoryItemsByCategory(itemsDetails);

                    categoryDetailsModel.GroupedCategoryItemsByCategory = groupedItems;
                }
            }
            else
            {
                var categories = await _categoriesToUserService.GetCategoriesThatHaveContent();

                categoryDetailsModel.Categories = categories;
            }

            return View(categoryDetailsModel);
            //IEnumerable<CategoryItemDetailsModel> categoryItemDetailsModels = null;
            //IEnumerable<GroupedCategoryItemsByCategoryModel> groupedCategoryItemsByCategoryModels = null;

            //CategoryDetailsModel categoryDetailsModel = new CategoryDetailsModel();

            //if (_signInManager.IsSignedIn(User))
            //{
            //    var user = await _userManager.GetUserAsync(User);

            //    if (user != null)
            //    {
            //        var userClaims = User.Claims;

            //        var userId = userClaims.ElementAt(0).Value;
            //        categoryItemDetailsModels = await GetCategoryItemDetailsForUser(userId);

            //        groupedCategoryItemsByCategoryModels =
            //            GetGroupedCategoryItemsByCategory(categoryItemDetailsModels);

            //        categoryDetailsModel.GroupedCategoryItemsByCategoryModels =
            //            groupedCategoryItemsByCategoryModels;
            //    }
            //}
            //else
            //{
            //    var categories = await GetCategoriesThatHaveContent();
            //    categoryDetailsModel.Categories = categories;
            //}
            //return View(categoryDetailsModel);
        }
  //      private async Task<List<Category>> GetCategoriesThatHaveContent()
  //      {
  //          var categoriesWithContent = await (from category in _context.Categories
  //                                             join categoryItem in _context.CategoryItems
  //                                             on category.Id equals categoryItem.CategoryId
  //                                             join content in _context.Content
  //                                             on categoryItem.Id equals content.CategoryItem.Id
  //                                             where content.IsActive== true
  //                                             where categoryItem.IsActive== true
  //                                             where category.IsActive== true
  //                                             select new Category
  //                                             {
  //                                                 Id = category.Id,
  //                                                 Title = category.Title,
  //                                                 Description = category.Description,
  //                                                 ThumbnailImagePath = category.ThumbnailImagePath
  //                                             }).Distinct().ToListAsync();
  //          return categoriesWithContent;
  //          //var categoriesWithContent = await (from category in _context.Categories
  //          //                                   join categoryItem in _context.CategoryItems
  //          //                                   on category.Id equals categoryItem.CategoryId
  //          //                                   join content in _context.Content
  //          //                                   on categoryItem.Id equals content.CategoryItem.Id
  //          //                                   select new Category
  //          //                                   {
  //          //                                       Id = category.Id,
  //          //                                       Title = category.Title,
  //          //                                       Description = category.Description,
  //          //                                       ThumbnailImagePath = category.ThumbnailImagePath
  //          //                                   }).Distinct().ToListAsync();
  //          //return categoriesWithContent;
  //      }

		//private IEnumerable<GroupedCategoryItemsByCategory> GetGroupedCategoryItemsByCategory(IEnumerable<CategoryItemDetailsModel> categoryItemDetailsModels)
  //      {
  //          return from item in categoryItemDetailsModels
  //                 group item by item.CategoryId into g
  //                 select new GroupedCategoryItemsByCategory()
  //                 {
  //                     Id = g.Key,
  //                     Title = g.Select(c => c.CategoryTitle).FirstOrDefault(),
  //                     Items = g
  //                 };
  //      }

  //      private async Task<IEnumerable<CategoryItemDetailsModel>> GetCategoryItemDetailsForUser(string userId)
  //      {
  //          return await (from catItem in _context.CategoryItems
  //                        join category in _context.Categories
  //                        on catItem.CategoryId equals category.Id
  //                        join content in _context.Content
  //                        on catItem.Id equals content.CategoryItem.Id
  //                        //join comments in _context.UserComments
  //                        //on content.Id equals comments.Content.Id
  //                        join userCat in _context.UserCategories
  //                        on category.Id equals userCat.CategoryId
  //                        join mediaType in _context.MediaTypes
  //                        on catItem.MediaTypeId equals mediaType.Id
  //                        where userCat.UserId == userId
  //                        where category.IsActive == true
  //                        where catItem.IsActive == true
  //                        where content.IsActive == true
  //                        select new CategoryItemDetailsModel()
  //                        {
  //                            CategoryId = category.Id,
  //                            CategoryTitle = category.Title,
  //                            CategoryItemId = catItem.Id,
  //                            CategoryItemTitle = catItem.Title,
  //                            CategoryItemDescription = catItem.Description,
  //                            MediaImagePath = mediaType.ThumbnailImagePath
  //                        })
  //                        .ToListAsync();

  //      }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
