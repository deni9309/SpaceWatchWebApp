using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Infrastructure.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceWatch.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: CategoryController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if(category==null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id, Title, Description, ThumbnailImagePath")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Title,Description,ThumbnailImagePath")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if(!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));   
            }
            return View(category);
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
            if(category==null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(x => x.Id == id);
        }
    }
}
