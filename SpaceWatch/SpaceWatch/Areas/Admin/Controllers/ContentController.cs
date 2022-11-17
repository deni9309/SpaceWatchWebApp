using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Content
        public async Task<IActionResult> Index()
        {
            return View(await _context.Content.ToListAsync());
        }

        // GET: Admin/Content/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            var catId = content.CategoryId;
            ViewBag.CategoryTitle = _context.Categories.FirstOrDefault(c => c.Id == catId).Title;

            ViewBag.CategoryItemTitle = content.CategoryItem.Title;

            return View(content);
        }

        // GET: Admin/Content/Create
        public IActionResult Create(int categoryItemId, int categoryId)
        {
            Content content = new Content
            {
                CategoryId = categoryId,
                CatItemId = categoryItemId
            };

            ViewBag.CategoryTitle = _context.Categories.FirstOrDefault(c => c.Id == categoryId).Title;
            ViewBag.CategoryItemTitle = _context.CategoryItems.FirstOrDefault(c => c.Id == categoryItemId).Title;
            return View(content);
        }

        // POST: Admin/Content/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,HtmlContent,VideoLink,CatItemId,CategoryId")] Content content)
        {
            if (ModelState.IsValid)
            {
                content.CategoryItem = await _context.CategoryItems.FindAsync(content.CatItemId);

                _context.Add(content);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), "CategoryItem", new { categoryId = content.CategoryId });
            }
            return View(content);
        }

        // GET: Admin/Content/Edit/5
        public async Task<IActionResult> Edit(int categoryId, int categoryItemId)
        {
            if (categoryItemId == 0)
            {
                return NotFound();
            }
            var content = await _context.Content
                .SingleOrDefaultAsync(c => c.CategoryItem.Id == categoryItemId);

            if (content == null)
            {
                return NotFound();
            }
            content.CategoryId = categoryId;

            ViewBag.CategoryTitle = _context.Categories.FirstOrDefault(c => c.Id == categoryId).Title;
            ViewBag.CategoryItemTitle = _context.CategoryItems.FirstOrDefault(c => c.Id == categoryItemId).Title;

            return View(content);
        }

        // POST: Admin/Content/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,HtmlContent,VideoLink,CategoryId")] Content content)
        {
            if (id != content.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(content);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(content.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "CategoryItem", new { categoryId = content.CategoryId });
            }
            return View(content);
        }

        // GET: Admin/Content/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            var catId = content.CategoryId;
            ViewBag.CategoryTitle = _context.Categories.FirstOrDefault(c => c.Id == catId).Title;
            
            ViewBag.CategoryItemTitle = content.CategoryItem.Title;

            return View(content);
        }

        // POST: Admin/Content/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var content = await _context.Content.FindAsync(id);
            _context.Content.Remove(content);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentExists(int id)
        {
            return _context.Content.Any(e => e.Id == id);
        }
    }
}
