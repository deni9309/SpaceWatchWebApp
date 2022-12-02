using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Infrastructure.Data;
using SpaceWatch.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceWatch.Controllers
{
    public class ContentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IContentService _contentService;
        private readonly ILogger<ContentController> _logger;

        public ContentController(ApplicationDbContext context,
            IContentService contentService,
            ILogger<ContentController> logger)
        {
            _context = context;
            _contentService = contentService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int categoryItemId)
        {

            Content content = await (from item in _context.Content
                                     where item.CategoryItem.Id == categoryItemId 
                                     where item.IsActive == true
                                     select new Content
                                     {
                                         Title = item.Title,
                                         VideoLink = item.VideoLink,
                                         HtmlContent = item.HtmlContent
                                     }).FirstOrDefaultAsync();
            return View(content);
        }
    }
}
