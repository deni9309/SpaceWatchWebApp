using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MediaTypeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IMediaTypeService _mediaTypeService;

        public MediaTypeController(ApplicationDbContext context, IMediaTypeService mediaTypeService)
        {
            _context = context;
            _mediaTypeService = mediaTypeService;
        }

        // GET: Admin/MediaType
        public async Task<IActionResult> Index()
        {
            //return View(await _context.MediaTypes.ToListAsync());
            return View(await _mediaTypeService.GetAll());
        }

        // GET: Admin/MediaType/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if ((await _mediaTypeService.MediaTypeExists(id)) == false)
            {
                return NotFound();
            }

            var model = await _mediaTypeService.MediaTypeDetailsById(id);
            //var mediaType = await _context.MediaTypes
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (mediaType == null)
            //{
            //    return NotFound();
            //}
            return View(model);
        }

        // GET: Admin/MediaType/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/MediaType/Create

        
        [HttpPost]
        public async Task<IActionResult> Create(MediaTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _mediaTypeService.Add(model);

            return RedirectToAction(nameof(Index));
        }
        //public async Task<IActionResult> Create([Bind("Id,Title,ThumbnailImagePath")] MediaType mediaType)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(mediaType);
        //    }

        //    _context.Add(mediaType);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Index", "MediaType");
        //}

        // GET: Admin/MediaType/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if ((await _mediaTypeService.MediaTypeExists(id) == false))
            {
                return NotFound();
            }
            var mediaType = await _mediaTypeService.MediaTypeDetailsById(id);

            var model = new MediaTypeViewModel()
            {
                Id = mediaType.Id,
                ThumbnailImagePath = mediaType.ThumbnailImagePath,
                Title = mediaType.Title
            };

            return View(model);
            //var mediaType = await _context.MediaTypes.FindAsync(id);
            //if (mediaType == null)
            //{
            //    return NotFound();
            //}
            //return View(mediaType);
        }

        // POST: Admin/MediaType/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, MediaTypeViewModel model)
        {
            if (id != model.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if ((await _mediaTypeService.MediaTypeExists(model.Id)) == false)
            {
                ModelState.AddModelError("", "Media Type does not exist!");

                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _mediaTypeService.Edit(model.Id, model);

            return RedirectToAction(nameof(Details), new { model.Id });
        }
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ThumbnailImagePath")] MediaType mediaType)
        //{
        //    if (id != mediaType.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(mediaType);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MediaTypeExists(mediaType.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(mediaType);
        //}

        // GET: Admin/MediaType/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if((await _mediaTypeService.MediaTypeExists(id)) == false)
            {
                return NotFound();
            }

            var mediaType = await _mediaTypeService.MediaTypeDetailsById(id);

            var model = new MediaTypeViewModel()
            {
                Id = mediaType.Id,
                ThumbnailImagePath = mediaType.ThumbnailImagePath,
                Title = mediaType.Title
            };

            return View(model);
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var mediaType = await _context.MediaTypes
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (mediaType == null)
            //{
            //    return NotFound();
            //}

            //return View(mediaType);
        }

        // POST: Admin/MediaType/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, MediaTypeViewModel model)
        {
            if ((await _mediaTypeService.MediaTypeExists(id)) == false)
            {
                return NotFound();
            }

            if (id != model.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await _mediaTypeService.Delete(id);

            return RedirectToAction(nameof(Index));
            //var mediaType = await _context.MediaTypes.FindAsync(id);
            //_context.MediaTypes.Remove(mediaType);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private bool MediaTypeExists(int id)
        {
            return _context.MediaTypes.Any(e => e.Id == id);
        }
    }
}
