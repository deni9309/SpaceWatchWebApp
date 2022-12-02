using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;

namespace SpaceWatch.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContentController : Controller
    {
		private readonly IContentService _contentService;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryItemService _categoryItemService;
        private readonly ILogger<ContentController> _logger;

		public ContentController(IContentService contentService,
            ICategoryService categoryService,
            ICategoryItemService categoryItemService,
			ILogger<ContentController> logger)
		{		
			_contentService = contentService;
			_logger = logger;
            _categoryService = categoryService;
            _categoryItemService = categoryItemService;
		}

		// GET: Admin/Content
		public async Task<IActionResult> Index()
        {
            return View(await _contentService.GetAll());
        }

        // GET: Admin/Content/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
			if ((await _contentService.ContentExists(id)) == false)
			{
				return NotFound();
			}

            var model = await _contentService.ContentDetailsById(id);

            return View(model);
			//if (id == null)
			//{
			//    return NotFound();
			//}

			//var content = await _context.Content
			//    .FirstOrDefaultAsync(m => m.Id == id);
			//if (content == null)
			//{
			//    return NotFound();
			//}

			//var catId = content.CategoryId;
			//ViewBag.CategoryTitle = _context.Categories.FirstOrDefault(c => c.Id == catId).Title;

			//ViewBag.CategoryItemTitle = content.CategoryItem.Title;

			//return View(content);
		}

        // GET: Admin/Content/Create
        [HttpGet]
        public async Task<IActionResult> Create(int categoryItemId, int categoryId)
        {
            if ((await _categoryService.CategoryExists(categoryId)) == false)
            {
                return NotFound();
            }

            if ((await _categoryItemService.CategoryItemExists(categoryItemId)) == false)
            {
                return NotFound();
            }

            var model = new ContentViewModel();

            model.CategoryId = categoryId;
            model.CatItemId = categoryItemId;            
            model.CategoryName = await _categoryService.GetCategoryTitleById(categoryId);

            return View(model);
            //Content content = new Content()
            //{
            //    CategoryId = categoryId,
            //    CatItemId = categoryItemId,
            //};

            //ViewBag.CategoryTitle = _context.Categories.FirstOrDefault(c => c.Id == categoryId).Title;
            //ViewBag.CategoryItemTitle = _context.CategoryItems.FirstOrDefault(c => c.Id == categoryItemId).Title;
            //return View(content);
        }

        // POST: Admin/Content/Create
        [HttpPost] //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContentViewModel model)  //[Bind("Id,Title,HtmlContent,VideoLink,CatItemId,CategoryId")] Content content
        {
            if ((await _categoryService.CategoryExists(model.CategoryId)) == false)
            {
                _logger.LogError(nameof(Create), "Category does not exists!");
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exists!");
            }

            if ((await _categoryItemService.CategoryItemExists(model.CatItemId)) == false)
            {
                _logger.LogError(nameof(Create), "Category Item does not exists!");
                ModelState.AddModelError(nameof(model.CatItemId), "Category Item does not exists!");
            }

            if(!ModelState.IsValid)
            {
                model.CategoryName = await _categoryService.GetCategoryTitleById(model.CategoryId);
                return View(model);
            }

            int id = await _contentService.Add(model);

            return RedirectToAction(nameof(Index), "CategoryItem", new { categoryId = model.CategoryId });
            //if (ModelState.IsValid)
            //{
            //    content.CategoryItem = await _context.CategoryItems.FindAsync(content.CatItemId);

            //    _context.Add(content);
            //    await _context.SaveChangesAsync();

            //    return RedirectToAction(nameof(Index), "CategoryItem", new { categoryId = content.CategoryId });
            //}
            //return View(content);
        }

        // GET: Admin/Content/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int categoryId, int categoryItemId)
        {
            if ((await _categoryService.CategoryExists(categoryId)) == false)
            {
                return NotFound();
            }

            if ((await _categoryItemService.CategoryItemExists(categoryItemId)) == false)
            {
                return NotFound();
            }

            var content = await _contentService.ContentDetailsByCategoryItemId(categoryItemId);

            var model = new ContentViewModel()
            {
                Id = content.Id,
                Title= content.Title,
                CategoryId = categoryId,
                CatItemId = content.CatItemId,
                CategoryName = await _categoryService.GetCategoryTitleById(categoryId),
                HtmlContent = content.HtmlContent,
                VideoLink = content.VideoLink,
            };

            return View(model);
            //if (categoryItemId == 0)
            //{
            //    return NotFound();
            //}
            //var content = await _context.Content
            //    .SingleOrDefaultAsync(c => c.CategoryItem.Id == categoryItemId);

            //if (content == null)
            //{
            //    return NotFound();
            //}
            //content.CategoryId = categoryId;

            //ViewBag.CategoryTitle = _context.Categories.FirstOrDefault(c => c.Id == categoryId).Title;
            //ViewBag.CategoryItemTitle = _context.CategoryItems.FirstOrDefault(c => c.Id == categoryItemId).Title;

            //return View(content);
        }

        // POST: Admin/Content/Edit/5
        [HttpPost] // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  ContentViewModel model)  // [Bind("Id,Title,HtmlContent,VideoLink,CategoryId")]
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if((await _contentService.ContentExists(model.Id.Value))==false)
            {             
                ModelState.AddModelError("", "Content does not exist!");
                model.CategoryId = _contentService.GetCategoryForContentAsync(model.Id.Value).Result.Id;
                model.CategoryName = _contentService.GetCategoryForContentAsync(model.Id.Value).Result.Title;
                return View(model);
            }

            if(!ModelState.IsValid)
            {
                model.CategoryId = _contentService.GetCategoryForContentAsync(model.Id.Value).Result.Id;
                model.CategoryName = _contentService.GetCategoryForContentAsync(model.Id.Value).Result.Title;
                return View(model);
            }

            await _contentService.Edit(model.Id.Value, model);

            return RedirectToAction(nameof(Index), "CategoryItem", new { categoryId = model.CategoryId });
            //if (id != content.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(content);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ContentExists(content.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index), "CategoryItem", new { categoryId = content.CategoryId });
            //}
            //return View(content);
        }

        // GET: Admin/Content/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var content = await _context.Content
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (content == null)
        //    {
        //        return NotFound();
        //    }

        //    var catId = content.CategoryId;
        //    ViewBag.CategoryTitle = _context.Categories.FirstOrDefault(c => c.Id == catId).Title;
            
        //    ViewBag.CategoryItemTitle = content.CategoryItem.Title;

        //    return View(content);
        //}

        //// POST: Admin/Content/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var content = await _context.Content.FindAsync(id);
        //    _context.Content.Remove(content);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ContentExists(int id)
        //{
        //    return _context.Content.Any(e => e.Id == id);
        //}
    }
}
