using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts.DefaultArea;
using SpaceWatch.Core.Models.DefaultArea;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWatch.Core.Services.DefaultArea
{
	public class ContentForUserService : IContentForUserService
	{
		private readonly IRepository _repo;
		private readonly ILogger<ContentForUserService> _logger;

		public ContentForUserService(IRepository repo, 
			ILogger<ContentForUserService> logger)
		{
			_repo = repo;
			_logger = logger;
		}

		public async Task<DisplayContentForUserViewModel> GetContent(int categoryItemId)
		{
			var content = await _repo.AllReadonly<Content>()
				.Where(c => c.IsActive)
				.Where(c => c.CatItemId == categoryItemId)
				.Select(c => new DisplayContentForUserViewModel()
				{
					Id = c.Id,
					Title = c.Title,
					VideoLink = c.VideoLink,
					HtmlContent = c.HtmlContent
				})
				.FirstAsync();

			return content;

			//Content content = await (from item in _context.Content
			//						 where item.CategoryItem.Id == categoryItemId
			//						 where item.IsActive == true
			//						 select new Content
			//						 {
			//							 Title = item.Title,
			//							 VideoLink = item.VideoLink,
			//							 HtmlContent = item.HtmlContent
			//						 }).FirstOrDefaultAsync();
		}
	}
}
