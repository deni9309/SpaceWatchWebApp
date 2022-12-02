using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts;
using SpaceWatch.Core.Models;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWatch.Core.Services
{
	public class CategoryItemService : ICategoryItemService
	{
		private readonly IRepository _repo;
		private readonly ILogger _logger;

		public CategoryItemService(IRepository repo,
			ILogger<CategoryItemService> logger)
		{
			_repo = repo;
			_logger = logger;
		}

		public async Task<int> Add(CategoryItemAddViewModel model)
		{
			var catItem = new CategoryItem()
			{
				Title = model.Title,
				Description = model.Description,
				CategoryId = model.CategoryId,
				MediaTypeId = model.MediaTypeId,
				DateTimeItemReleased = model.DateTimeItemReleased
			};

			try
			{
				await _repo.AddAsync(catItem);
				await _repo.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(nameof(Add), ex);
				throw new ApplicationException("Database failed to save info.", ex);
			}

			return catItem.Id;
		}

		public Task<CategoryItemViewModel> CategoryItemDetailsById(int id)
		{
			return _repo.AllReadonly<CategoryItem>()
				.Where(c => c.IsActive)
				.Where(c => c.Id == id)
				.Select(c => new CategoryItemViewModel()
				{
					Title = c.Title,
					Description = c.Description,
					CategoryId = c.CategoryId,
					MediaTypeId = c.MediaTypeId,
					DateTimeItemReleased = c.DateTimeItemReleased,
					Id = c.Id
				})
				.FirstAsync();
		}

		public async Task<bool> CategoryItemExists(int categoryItemId)
		{
			return await _repo.AllReadonly<CategoryItem>()
			   .AnyAsync(c => c.Id == categoryItemId && c.IsActive);
		}

		public async Task Delete(int categoryItemId)
		{
			var categoryItem = await _repo.GetByIdAsync<CategoryItem>(categoryItemId);

			try
			{
				if(categoryItem != null)
				{
					await DeleteReletedContent(categoryItemId);

					categoryItem.IsActive = false;
					
					await _repo.SaveChangesAsync();
				}
			}
			catch (ArgumentNullException ex)
			{
				_logger.LogError(nameof(Delete), ex);
				throw new ArgumentNullException(nameof(categoryItemId));
			}
		
		}

		/// <summary>
		/// Executes only if releted content exists.
		/// Sets IsActive property to false on content item when its principal entity is set to so.
		/// </summary>
		/// <param name="categoryItemId"></param>
		/// <returns></returns>
		public async Task DeleteReletedContent(int categoryItemId)
		{
			if (await _repo.AllReadonly<Content>().AnyAsync(c => c.CatItemId == categoryItemId)) 
			{
                var content = await _repo.All<Content>()
                .Where(c => c.CatItemId == categoryItemId)
                .FirstAsync();

				content.IsActive = false;

				await _repo.SaveChangesAsync();
            }
		}

		public async Task<int> Edit(int categoryItemId, CategoryItemAddViewModel model)
		{
			var categoryItem = await _repo.GetByIdAsync<CategoryItem>(categoryItemId);

			categoryItem.Title = model.Title;
			categoryItem.Description = model.Description;
			categoryItem.CategoryId = model.CategoryId;
			categoryItem.MediaTypeId = model.MediaTypeId;
			categoryItem.DateTimeItemReleased = model.DateTimeItemReleased;

			try
			{
				await _repo.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				if (CategoryItemExists(categoryItem.Id).Result == false)
				{
					_logger.LogError(nameof(Edit), ex);
					throw new ApplicationException("Category Item could not be found in database.", ex);
				}

				_logger.LogError(nameof(Edit), ex);
				throw new DbUpdateException("Database failed to update info.", ex);
			}

			return categoryItem.Id;
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

		public async Task<string> GetCategoryTitleByCatItemId(int categoryItemId)
		{
			return await _repo.AllReadonly<CategoryItem>()
				.Where(c => c.IsActive)
				.Include(c => c.Category)
				.Where(c => c.Id == categoryItemId)
				.Select(ca => ca.Category.Title).FirstAsync();

		}

		//private int GetContentIdForCategoryItem(int categoryItemId)
		//{
		//	//var content = _repo.AllReadonly<Content>()				
		//	//	.Where(c => c.IsActive)
		//	//	.FirstOrDefault(c => c.CatItemId == categoryItemId);
		//	var content = _repo.AllReadonly<Content>().Where(co => co.CatItemId == categoryItemId).FirstOrDefault();
		//	if (content != null)
		//	{
		//		return content.Id;
		//	}
		//	return 0;
		//}
		//List<CategoryItem> list = await (from catItem in _context.CategoryItems
		//								 join contentItem in _context.Content
		//								 on catItem.Id equals contentItem.CategoryItem.Id
		//								 into gj
		//from subContent in gj.DefaultIfEmpty()

		//								 where catItem.CategoryId == categoryId
		//								 select new CategoryItem
		//								 {
		//									 Id = catItem.Id,
		//									 Title = catItem.Title,
		//									 Description = catItem.Description,
		//									 DateTimeItemReleased = catItem.DateTimeItemReleased,
		//									 MediaTypeId = catItem.MediaTypeId,
		//									 CategoryId = categoryId,
		//									 ContentId = (subContent != null) ? subContent.Id : 0
		//								 }).ToListAsync();

		//ViewBag.CategoryId = categoryId;

		//ViewBag.CategoryTitle = _context.Categories
		//	.FirstOrDefaultAsync(c => c.Id == categoryId).Result.Title;
	} 
}
