using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;

namespace SpaceWatch.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MediaTypeController : Controller
    {
        private readonly IMediaTypeService _mediaTypeService;

        public MediaTypeController(IMediaTypeService mediaTypeService)
        {
            _mediaTypeService = mediaTypeService;
        }

        // GET: Admin/MediaType
        public async Task<IActionResult> Index()
        {
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
        }

        // POST: Admin/MediaType/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, MediaTypeViewModel model)
        {
            if (id != model.Id)
            {
                return RedirectToPage("/Pages/AccessDenied", new { area = "Identity" });
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
                return RedirectToPage("/Pages/AccessDenied", new { area = "Identity" });
            }

            await _mediaTypeService.Delete(id);

            return RedirectToAction(nameof(Index));         
        }
    }
}
