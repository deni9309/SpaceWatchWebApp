using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using SpaceWatch.Extensions;

namespace SpaceWatch.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryItemController : Controller
    {
        private readonly ICategoryItemService _categoryItemService;
        private readonly ICategoryService _categoryService;
		private readonly IMediaTypeService _mediaTypeService;
        private readonly ILogger<CategoryItemController> _logger;

        public CategoryItemController(ICategoryItemService categoryItemService, 
            ICategoryService categoryService,
            IMediaTypeService mediaTypeService,
            ILogger<CategoryItemController> logger)
        {            
            _categoryItemService = categoryItemService;
            _categoryService = categoryService;
            _mediaTypeService = mediaTypeService;
            _logger = logger;
        }

        // GET: Admin/CategoryItem
        public async Task<IActionResult> Index(int categoryId)
        {
            var model = await _categoryItemService.GetAllCategoryItemsFromCategory(categoryId);

            ViewBag.CategoryId = categoryId;
			ViewBag.CategoryTitle = await _categoryService.GetCategoryTitleById(categoryId);
	
			return View(model);
        }

        // GET: Admin/CategoryItem/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if ((await _categoryItemService.CategoryItemExists(id)) == false)
            {
                return NotFound();
            }

            var model = await _categoryItemService.CategoryItemDetailsById(id);

            if (model == null)
            {
                return NotFound();
            }

			TempData["CategoryTitle"] = await _categoryItemService.GetCategoryTitleByCatItemId(id);
            return View(model);
		}

		// GET: Admin/CategoryItem/Create
		[HttpGet]
        public async Task<IActionResult> Create(int categoryId)
        {
            var model = new CategoryItemAddViewModel()
            {
                CategoryId = categoryId,
                MediaTypes = await _mediaTypeService.GetMediaTypesForSelectList()
            };

            TempData["CategoryId"] = categoryId;
            TempData["CategoryTitle"] = await _categoryService.GetCategoryTitleById(categoryId);

            return View(model);
		}

        [HttpPost]
		public async Task<IActionResult> Create(CategoryItemAddViewModel model)
        {
			if (!ModelState.IsValid)
            {
                model.MediaTypes = await _mediaTypeService.GetMediaTypesForSelectList();
                TempData["CategoryTitle"] = await _categoryService.GetCategoryTitleById(model.CategoryId);
				return View(model);
			}

            int id = await _categoryItemService.Add(model);
			
			return RedirectToAction(nameof(Index), new { categoryId = model.CategoryId });
		}

        // GET: Admin/CategoryItem/Edit/5
        [HttpGet]
		public async Task<IActionResult> Edit(int id)
        {
            if ((await _categoryItemService.CategoryItemExists(id)) == false)
            {
                return NotFound();
            }

            var categoryItem = await _categoryItemService.CategoryItemDetailsById(id);

            var model = new CategoryItemAddViewModel()
            {
                Id = categoryItem.Id,
                Title = categoryItem.Title,
                DateTimeItemReleased = categoryItem.DateTimeItemReleased,
                Description = categoryItem.Description,
                CategoryId = categoryItem.CategoryId,
                MediaTypeId = categoryItem.MediaTypeId,
                MediaTypes = await _mediaTypeService.GetMediaTypesForSelectList()
            };

            TempData["CategoryId"] = model.CategoryId;
            TempData["CategoryTitle"] = await _categoryService.GetCategoryTitleById(model.CategoryId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CategoryItemAddViewModel model)
        {
            if (id != model.Id)
            {
                return RedirectToAction(nameof(Index), new { id = model.CategoryId });
            }

            if ((await _categoryItemService.CategoryItemExists(model.Id.Value)) == false)
            {
                model.MediaTypes = await _mediaTypeService.GetMediaTypesForSelectList();
                ModelState.AddModelError("", "Category Item does not exists!");

                TempData["CategoryTitle"] = await _categoryService.GetCategoryTitleById(model.CategoryId);

                return View(model);
            }

            if (!ModelState.IsValid)
            {
                model.MediaTypes = await _mediaTypeService.GetMediaTypesForSelectList();

                TempData["CategoryTitle"] = await _categoryService.GetCategoryTitleById(model.CategoryId);

                return View(model);
            }

            int itemId = await _categoryItemService.Edit(model.Id.Value, model);

            return RedirectToAction(nameof(Details), new { id = itemId });
        }

        // GET: Admin/CategoryItem/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await _categoryItemService.CategoryItemExists(id)) == false)
            {
                return NotFound();
            }

            var categoryItem = await _categoryItemService.CategoryItemDetailsById(id);
            var model = new CategoryItemViewModel()
            {
                CategoryId = categoryItem.CategoryId,
                MediaTypeId = categoryItem.MediaTypeId,
                Id = categoryItem.Id,
                Title = categoryItem.Title,
                Description = categoryItem.Description,
                DateTimeItemReleased = categoryItem.DateTimeItemReleased
            };

            TempData["CategoryTitle"] = await _categoryService.GetCategoryTitleById(model.CategoryId);

            return View(model);
        }

        // POST: Admin/CategoryItem/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, CategoryItemViewModel model)
        {
            if(id != model.Id)
            {
                _logger.LogInformation("User with id {0} attempts to access category item he has no rights over.", User.GetLoggedInUserId<string>());
                return NotFound();
            }

            if ((await _categoryItemService.CategoryItemExists(id)) == false)
            {
                return NotFound();
            }

            int categoryId = model.CategoryId;

            await _categoryItemService.Delete(id);

            if((_categoryItemService.GetAllCategoryItemsFromCategory(model.CategoryId).Result.Any()))
            {
                return RedirectToAction(nameof(Index), new { model.CategoryId });
            }

            return RedirectToAction(nameof(Index), "Category");
        }
    }
}
