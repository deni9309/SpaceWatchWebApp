using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using System.Threading.Tasks;

namespace SpaceWatch.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            return View(await _categoryService.GetAll());
        }

        // GET: CategoryController/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            if((await _categoryService.CategoryExists(id)) == false)
            {
                return RedirectToAction("Index");
            }

            var model = await _categoryService.CategoryDetailsById(id);

            return View(model);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var category = await _context.Categories
            //    .FirstOrDefaultAsync(c => c.Id == id);

            //if(category==null)
            //{
            //    return NotFound();
            //}
            //return View(category);
        }

        // GET: CategoryController/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new CategoryViewModel();

            return View(model);
        }

        // POST: CategoryController/Create
        [HttpPost]
        public async Task<ActionResult> Create(CategoryViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            await _categoryService.Add(model);

            return RedirectToAction(nameof(Index));
            //if (ModelState.IsValid)
            //{
            //    _context.Add(category);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(category);
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if ((await _categoryService.CategoryExists(id)) == false)
            {
                return NotFound();
            }

            var category = await _categoryService.CategoryDetailsById(id);

            var model = new CategoryViewModel()
            {
                Id = category.Id,
                ThumbnailImagePath = category.ThumbnailImagePath,
                Title = category.Title,
                Description = category.Description
            };

            return View(model);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, CategoryViewModel model)
        {
            if(id != model.Id)
            {
                return RedirectToPage("/Pages/AccessDenied", new { area = "Identity" });
            }

            if ((await _categoryService.CategoryExists(model.Id)) == false)
            {
                ModelState.AddModelError("", "Category does not exist!");
                return View(model);
            }
           
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _categoryService.Edit(model.Id, model);

            return RedirectToAction(nameof(Details), new { model.Id });
        }

        // GET: CategoryController/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            if ((await _categoryService.CategoryExists(id)) == false)
            {
                return NotFound();
            }

            var category = await _categoryService.CategoryDetailsById(id);
            var model = new CategoryViewModel()
            {
                Id = category.Id,
                ThumbnailImagePath = category.ThumbnailImagePath,
                Title = category.Title,
                Description = category.Description
            };

            return View(model);
        }

        // POST: CategoryController/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id, CategoryViewModel model)
        {
            if ((await _categoryService.CategoryExists(id)) == false)
            {
                return NotFound();
            }
            if (id != model.Id)
            {
                return RedirectToPage("/Pages/AccessDenied", new { area = "Identity" });
            }

            await _categoryService.Delete(id);
          
            return RedirectToAction(nameof(Index));
        }
    }
}
