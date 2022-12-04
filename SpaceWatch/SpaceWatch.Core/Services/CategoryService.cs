using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Core.Services
{
	public class CategoryService : ICategoryService
	{
		private IRepository _repo;
		private ILogger<CategoryService> _logger;

		public CategoryService(IRepository repo,
			ILogger<CategoryService> logger)
		{
			_repo = repo;
			_logger = logger;
		}

		public async Task Add(CategoryViewModel model)
		{
			var category = new Category()
			{
				ThumbnailImagePath= model.ThumbnailImagePath,
				Title= model.Title,
				Description= model.Description,
			};

			await _repo.AddAsync(category);
			await _repo.SaveChangesAsync();
		}

		public async Task<CategoryViewModel> CategoryDetailsById(int id)
		{
			return await _repo.AllReadonly<Category>()
				.Where(c=> c.IsActive)
				.Where(c => c.Id == id)
				.Select(c => new CategoryViewModel()
				{
					Id = c.Id,
					Title = c.Title,
					Description = c.Description,
					ThumbnailImagePath= c.ThumbnailImagePath
				})
				.FirstAsync();
		}

		public async Task<bool> CategoryExists(int categoryId)
		{
			return await _repo.AllReadonly<Category>()
				.AnyAsync(c=> c.Id == categoryId && c.IsActive);
		}

		public async Task<string> GetCategoryTitleById(int categoryId)
		{
			var category = await _repo.GetByIdAsync<Category>(categoryId);

			return category.Title;
				
		}

		public async Task Delete(int categoryId)
		{
			await DeleteReletedCategoryItemsAndContentFromCategory(categoryId);

			var category = await _repo.GetByIdAsync<Category>(categoryId);
		
			category.IsActive = false;

			await _repo.SaveChangesAsync();
		}

		public async Task DeleteReletedCategoryItemsAndContentFromCategory(int categoryId)
		{
			try
			{
				var categoryItemIds = await _repo.All<CategoryItem>()
					.Include(c => c.Category)
					.Where(c => c.CategoryId == categoryId && c.IsActive)
					.Select(c => c.Id)
					.ToListAsync();

				if (categoryItemIds.Any())
				{

					await _repo.All<Content>()
						.Where(c => c.IsActive && categoryItemIds.Contains(c.CatItemId))
						.ForEachAsync(c => c.IsActive = false);
				}

				await _repo.All<CategoryItem>()
					.Include(c => c.Category)
					.Where(c => c.CategoryId == categoryId && c.IsActive)
					.ForEachAsync(item => item.IsActive = false);

				await _repo.SaveChangesAsync();

			}
			catch (Exception ex) 
			{
				_logger.LogError(nameof(DeleteReletedCategoryItemsAndContentFromCategory), ex);
				throw new ApplicationException("Something went wrong while trying to delete records from database!", ex);
			}
		}

        public async Task<IEnumerable<CategoryItemViewModel>> GetAllCategoryItemsFromCategory(int categoryId)
        {
            var categoryItems = await _repo.AllReadonly<CategoryItem>()
                .Include(c => c.Category)
                .Where(c => c.CategoryId == categoryId && c.IsActive)
                .Select(c => new CategoryItemViewModel()
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.Category.Title,
                    MediaTypeId = c.MediaTypeId,
                    Id = c.Id,
                    Title = c.Title,
                    DateTimeItemReleased = c.DateTimeItemReleased,
                    Description = c.Description,
                    ContentId = (_repo.AllReadonly<Content>().Any(co => co.CatItemId == c.Id) ?
                    (_repo.AllReadonly<Content>().First(co => co.CatItemId == c.Id).Id) : 0)
                    //GetContentIdForCategoryItem(c.Id)
                })
                .ToListAsync();

            return categoryItems;
        }
      
        public async Task Edit(int categoryId, CategoryViewModel model)
		{
			var category = await _repo.GetByIdAsync<Category>(categoryId);

			category.Title = model.Title;
			category.Description = model.Description;
			category.ThumbnailImagePath = model.ThumbnailImagePath;

			await _repo.SaveChangesAsync();	
		}

		public async Task<IEnumerable<CategoryViewModel>> GetAll()
		{
			return await _repo.AllReadonly<Category>()
				.Where(c => c.IsActive)
				.Select(c => new CategoryViewModel()
				{
					ThumbnailImagePath = c.ThumbnailImagePath,
					Description = c.Description,
					Title = c.Title,
					Id = c.Id
				})
				.ToListAsync();
		}
	}
}
