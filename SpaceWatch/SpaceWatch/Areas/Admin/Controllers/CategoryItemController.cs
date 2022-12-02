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
        //private readonly ApplicationDbContext _context;
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
            //List<CategoryItem> list = await (from catItem in _context.CategoryItems
            //                                 join contentItem in _context.Content
            //                                 on catItem.Id equals contentItem.CategoryItem.Id
            //                                 into gj
            //                                 from subContent in gj.DefaultIfEmpty()

            //                                 where catItem.CategoryId == categoryId
            //                                 select new CategoryItem
            //                                 {
            //                                     Id = catItem.Id,
            //                                     Title = catItem.Title,
            //                                     Description = catItem.Description,
            //                                     DateTimeItemReleased = catItem.DateTimeItemReleased,
            //                                     MediaTypeId = catItem.MediaTypeId,
            //                                     CategoryId = categoryId,
            //                                     ContentId = (subContent != null) ? subContent.Id : 0
            //                                 }).ToListAsync();
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
			//if (id == null)
			//{
			//    return NotFound();
			//}

			//var categoryItem = await _context.CategoryItems
			//    .FirstOrDefaultAsync(m => m.Id == id);

			//if (categoryItem == null)
			//{
			//    return NotFound();
			//}
			//return View(categoryItem);
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
			//List<MediaType> mediaTypes = await _context.MediaTypes.ToListAsync();

			//CategoryItem categoryItem = new CategoryItem
			//{
			//    CategoryId = categoryId,
			//    MediaTypes = mediaTypes.ConvertToSelectList(0)
			//};

			//ViewBag.CategoryTitle = _context.Categories
			//   .FirstOrDefaultAsync(c => c.Id == categoryId).Result.Title;

			//return View(categoryItem);
		}

        [HttpPost]
		//[ValidateAntiForgeryToken]
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
        // POST: Admin/CategoryItem/Create       
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(
        //    [Bind("Id,Title,Description,CategoryId,MediaTypeId,DateTimeItemReleased")] CategoryItem categoryItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(categoryItem);
        //        await _context.SaveChangesAsync();

        //        return RedirectToAction(nameof(Index), new { categoryId = categoryItem.CategoryId });

        //    }
        //    List<MediaType> mediaTypes = await _context.MediaTypes.ToListAsync();
        //    categoryItem.MediaTypes = mediaTypes.ConvertToSelectList(categoryItem.MediaTypeId);

        //    return View(categoryItem);
        //}

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
            //List<MediaType> mediaTypes = await _context.MediaTypes.ToListAsync();

            //var categoryItem = await _context.CategoryItems.FindAsync(id);

            //if (categoryItem == null)
            //{
            //    return NotFound();
            //}
            //categoryItem.MediaTypes = mediaTypes.ConvertToSelectList(categoryItem.MediaTypeId);

            //var catId = categoryItem.CategoryId;
            //ViewBag.CategoryTitle = _context.Categories.FirstOrDefault(c => c.Id == catId).Title;

            //return View(categoryItem);
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

        // POST: Admin/CategoryItem/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, 
        //    [Bind("Id,Title,Description,CategoryId,MediaTypeId,DateTimeItemReleased")] CategoryItem categoryItem)
        //{
        //    if (id != categoryItem.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(categoryItem);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CategoryItemExists(categoryItem.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index), new { categoryId = categoryItem.CategoryId });
        //    }
        //    return View(categoryItem);
        //}

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
            //var categoryItem = await _context.CategoryItems
            //    .FirstOrDefaultAsync(m => m.Id == id);

            //if (categoryItem == null)
            //{
            //    return NotFound();
            //}

            //var catId = categoryItem.CategoryId;
            //ViewBag.CategoryTitle = _context.Categories.FirstOrDefault(c => c.Id == catId).Title;

            //return View(categoryItem);
        }

        // POST: Admin/CategoryItem/Delete/5
        [HttpPost, ActionName("Delete")] //[ValidateAntiForgeryToken]
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

            //var categoryItem = await _context.CategoryItems
            //    .FindAsync(id);

            //_context.CategoryItems.Remove(categoryItem);
            //await _context.SaveChangesAsync();

            //return RedirectToAction(nameof(Index), new { categoryId = categoryItem.CategoryId });
        }

        //private bool CategoryItemExists(int id)
        //{
        //    return _context.CategoryItems.Any(e => e.Id == id);
        //}
    }
}
