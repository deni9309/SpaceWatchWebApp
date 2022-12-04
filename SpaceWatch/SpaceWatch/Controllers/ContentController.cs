using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Contracts.DefaultArea;
using System.Threading.Tasks;

namespace SpaceWatch.Controllers
{
    [Authorize]
    public class ContentController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IContentService _contentService;
        IContentForUserService _contentForUserService;

		private readonly ILogger<ContentController> _logger;

        public ContentController(/*ApplicationDbContext context,*/
            IContentService contentService,
            IContentForUserService contentForUserService,
            ILogger<ContentController> logger)
        {
            //_context = context;
            _contentService = contentService;
            _contentForUserService = contentForUserService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int categoryItemId)
        {
            var model = await _contentForUserService.GetContent(categoryItemId);
            if (model == null)
            {
                _logger.LogInformation(nameof(Index), "User tries to access content that could not be found.");

                return NotFound();
            }

            var contentDetails = _contentService.ContentDetailsByCategoryItemId(categoryItemId).Result;
            TempData["CategoryTitle"] = contentDetails.CategoryName;

            return View(model);
        }
    }
}
