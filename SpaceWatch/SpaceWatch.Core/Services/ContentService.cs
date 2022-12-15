using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Core.Services
{
	public class ContentService : IContentService
	{
		private readonly IRepository _repo;
		private readonly ILogger<ContentService> _logger;

		public ContentService(IRepository repo,
			ILogger<ContentService> logger)
		{
			_repo = repo;
			_logger = logger;
		}

		public async Task<int> Add(ContentViewModel model)
		{
			var content = new Content()
			{
				Title = model.Title,
				CatItemId = model.CatItemId,
				HtmlContent = model.HtmlContent,
				VideoLink = model.VideoLink,
				CategoryId = model.CategoryId
			};

			try 
			{
				await _repo.AddAsync(content);
				await _repo.SaveChangesAsync();
			}
			catch(Exception ex) 
			{
                _logger.LogError(nameof(Add), ex);
                throw new ApplicationException("Database failed to save info.", ex);
            }

			return content.Id;
		}

        public async Task<ContentViewModel> ContentDetailsByCategoryItemId(int categoryItemId)
        {
			var contetModel = await _repo.AllReadonly<Content>()
				.Include(c => c.CategoryItem)
				.Include(c => c.CategoryItem.Category)
				 .Where(c => c.IsActive)
				 .Where(c => c.CatItemId == categoryItemId)
				 .Select(c => new ContentViewModel()
				 {
					 Id = c.Id,
					 Title = c.Title,
					 VideoLink = c.VideoLink,
					 HtmlContent = c.HtmlContent,
					 CatItemId = c.CatItemId,
					 CategoryId = c.CategoryItem.Category.Id,
					 CategoryName = c.CategoryItem.Category.Title
				 })
				 .FirstOrDefaultAsync();

			if(contetModel == null)
			{
				_logger.LogError(nameof(ContentDetailsByCategoryItemId));
				throw new ApplicationException("Could not find Content details for given categoryItemId");
			}

			return contetModel;
        }

        public async Task<ContentViewModel> ContentDetailsById(int id)
		{
			var content = await _repo.AllReadonly<Content>()
					.Where(c => c.IsActive)
					.Where(c => c.Id == id)
					.Select(c => new ContentViewModel()
					{
						Id = c.Id,	
						Title = c.Title,
						VideoLink = c.VideoLink,
						HtmlContent = c.HtmlContent,
						CategoryId = GetCategoryForContentAsync(id).Result.Id,
						CategoryName = GetCategoryForContentAsync(id).Result.Title
					})
					.FirstOrDefaultAsync();

			if(content == null)
			{
				_logger.LogError(nameof(ContentDetailsById));
				throw new Exception("Could not find Content details for given Id");
			}

			return content;
		}

		public async Task<bool> ContentExists(int ContentId)
		{
			return await _repo.AllReadonly<Content>()
				.Where(c => c.IsActive)
				.Where(c => c.Id == ContentId)
				.AnyAsync();
		}

		public async Task Delete(int id)
		{
			var content = await _repo.GetByIdAsync<Content>(id);

			content.IsActive = false;

			await _repo.SaveChangesAsync();
		}

		public async Task Edit(int id, ContentViewModel model)
		{
            var content = await _repo.GetByIdAsync<Content>(id);

			content.Title = model.Title;
			content.VideoLink = model.VideoLink;
			content.HtmlContent = model.HtmlContent;
			content.CatItemId = model.CatItemId;

			await _repo.SaveChangesAsync();
        }

		public async Task<IEnumerable<ContentViewModel>> GetAll()
		{
			return await _repo.AllReadonly<Content>()
				.Where(c => c.IsActive)
				.Select(c => new ContentViewModel()
				{
					Id = c.Id,
					CatItemId= c.CatItemId,
					Title = c.Title,
					VideoLink = c.VideoLink,
					HtmlContent = c.HtmlContent,
					CategoryId = GetCategoryForContentAsync(c.Id).Result.Id,
					CategoryName = GetCategoryForContentAsync(c.Id).Result.Title				
				})
				.ToListAsync();
		}

		public async Task<CategoryViewModel> GetCategoryForContentAsync(int contentId)
		{
			Content content;
			Category category;
			CategoryViewModel categoryForContent;
			try
			{
				content = await _repo.AllReadonly<Content>()
					.Where(c => c.IsActive)
					.FirstAsync(c => c.Id == contentId);

				category = await _repo.AllReadonly<Category>()
					.Include(c => c.CategoryItems)
					.Where(c => c.IsActive)
					.Select(c => c.CategoryItems.FirstOrDefault(i => i.Id == content.CatItemId)).Select(ca => ca.Category)
					.FirstAsync();

				categoryForContent = new CategoryViewModel()
				{
					Id = category.Id,
					ThumbnailImagePath = category.ThumbnailImagePath,
					Description = category.Description,
					Title = category.Title
				};
			}
			catch(Exception ex)
			{
				_logger.LogError(nameof(GetCategoryForContentAsync), ex);
				throw new ApplicationException("An error occured while trying to fetch category info for content!");
			}

			

			return categoryForContent;
		}

		//public async Task<string> GetCatItemNameAsync(int contentId)
		//{
		//	return await _repo.AllReadonly<Content>()
		//		.Include(c => c.CategoryItem)
		//		.Where(c => c.IsActive)
		//		.Where(c => c.Id == contentId)
		//		.Select(c => c.CategoryItem.Title)
		//		.FirstOrDefaultAsync();
		//}
	}
}
