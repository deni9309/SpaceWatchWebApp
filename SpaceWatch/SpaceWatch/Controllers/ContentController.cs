using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Contracts.DefaultArea;
using SpaceWatch.Core.Models.DefaultArea;
using SpaceWatch.Core.Services;
using SpaceWatch.Extensions;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Infrastructure.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceWatch.Controllers
{
    [Authorize]
    public class ContentController : Controller
    {
        private readonly IContentService _contentService;
        private readonly IContentForUserService _contentForUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ILogger<ContentController> _logger;

        public ContentController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IContentService contentService,
            IContentForUserService contentForUserService,
            ILogger<ContentController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _contentService = contentService;
            _contentForUserService = contentForUserService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int categoryItemId)
        {
            var model = await _contentForUserService.GetContent(categoryItemId);

            if (model == null)
            {
                _logger.LogInformation(nameof(Index), "User tries to access content that could not be found.");
                return NotFound();
            }

            model.Comments = await _contentForUserService.GetCommentsForContent(model.Id);

			var contentDetails = _contentService.ContentDetailsByCategoryItemId(categoryItemId).Result;
            TempData["CategoryTitle"] = contentDetails.CategoryName;
            return View(model);
        }

   //     [HttpGet]
   //     public async Task<IActionResult> CreateComment(int contentId, int cattegoryItemId)
   //     {
			//if ((await _contentService.ContentExists(contentId)) == false)
   //         {
   //             _logger.LogError(nameof(CreateComment), "Content to be commented does not exist in database.");

   //             return NotFound();
   //         }

   //         if ((_signInManager.IsSignedIn(User) == false))
   //         {
   //             _logger.LogInformation(nameof(CreateComment), "Not signed in user tries to comment a content.");
   //             return RedirectToAction("Login", "UserAuth");
   //         }

			//var user = await _userManager.FindByIdAsync(User.Claims.ElementAt(0).Value);
   //         var userId = user?.Id;

			//var model = new CommentAddViewModel()
   //         {
   //             UserId = userId,
   //             ContentId = contentId,
   //             CategoryItemId = cattegoryItemId
   //         };

   //         return PartialView("_CreateCommentPartial", model);
   //     }

        [HttpPost]
        public async Task<IActionResult> CreateComment(int contentId, int categoryItemId, CommentAddViewModel model)
        {
			var user = await _userManager.FindByIdAsync(User.Claims.ElementAt(0).Value);
			
            if (user == null)
			{
				_logger.LogError(nameof(CreateComment), "User could not be found in database.");
				return RedirectToAction("Login", "UserAuth");
			}

			if ((await _contentService.ContentExists(model.ContentId)) == false)
            {
                _logger.LogError(nameof(CreateComment), "Content to be commented does not exist in database.");
                return NotFound();
            }

			string userId = user.Id;

			model.UserId = userId;
			//model.ContentId = contentId;
			//model.CategoryItemId = categoryItemId;

            if(!ModelState.IsValid)
            {
				model.UserId = userId;
				model.ContentId = contentId;
				model.CategoryItemId = categoryItemId;
                ModelState.AddModelError(string.Empty, "Please write your message up to 500 characters long!");
				return RedirectToAction(nameof(Index), new { categoryItemId = model.CategoryItemId });
			}

            await _contentForUserService.AddComment(model);

            return RedirectToAction(nameof(Index), new { categoryItemId = model.CategoryItemId });
        }
    }
}
