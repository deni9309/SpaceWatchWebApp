using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaceWatch.Core.Contracts.DefaultArea;
using SpaceWatch.Core.Models;
using SpaceWatch.Infrastructure.Common;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Core.Services.DefaultArea
{
	public class CategoriesToUserService : ICategoriesToUserService
	{
		private readonly IRepository _repo;
		private readonly ILogger<CategoriesToUserService> _logger;

		public CategoriesToUserService(IRepository repository,
			ILogger<CategoriesToUserService> logger)
		{
			_repo= repository;
			_logger= logger;
		}

		public async Task<ICollection<CategoryViewModel>> GetCategoriesCurrentlySavedForUser(string userId)
		{
			List<CategoryViewModel> categoriesCurrentlySavedForUser = new List<CategoryViewModel>();

			try
			{
				categoriesCurrentlySavedForUser = await _repo.All<UserCategory>()
					.Include(uc => uc.Category)
					.Include(c => c.Category.CategoryItems)
					.Where(c => c.UserId == userId)
					.Where(c => c.Category.IsActive && c.Category.CategoryItems.Any(i => i.IsActive))
					.Select(c => new CategoryViewModel()
					{
						Id = c.CategoryId
					})
					.ToListAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(nameof(GetCategoriesCurrentlySavedForUser), ex);
				throw new ApplicationException("Could not retrieve information about categories user is currently assigned to.", ex);
			}

			return categoriesCurrentlySavedForUser;

			//var categoriesCurrentlySavedForUser = await (from userCategory in _context.UserCategories
			//											 join cat in _context.Categories
			//											 on userCategory.CategoryId equals cat.Id
			//											 join catItem in _context.CategoryItems
			//											 on cat.Id equals catItem.CategoryId
			//											 where catItem.IsActive == true
			//											 where userCategory.UserId == userId
			//											 where userCategory.Category.IsActive == true
			//											 select new Category
			//											 {
			//												 Id = userCategory.CategoryId
			//											 }).ToListAsync();
			//return categoriesCurrentlySavedForUser;
		}

		public async Task<ICollection<CategoryViewModel>> GetCategoriesThatHaveContent()
		{
			List<CategoryViewModel> categoriesThatHaveContent = new List<CategoryViewModel>();

			try
			{
				categoriesThatHaveContent = await (from category in _repo.All<Category>()
												   join categoryItem in _repo.All<CategoryItem>()
												   on category.Id equals categoryItem.CategoryId
												   join content in _repo.All<Content>()
												   on categoryItem.Id equals content.CategoryItem.Id
												   where category.IsActive == true
												   where categoryItem.IsActive == true
												   where content.IsActive == true
												   select new CategoryViewModel()
												   {
													   Id = category.Id,
													   Title = category.Title,
													   Description = category.Description,
													   ThumbnailImagePath = category.ThumbnailImagePath
												   })
												   .Distinct()
												   .ToListAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(nameof(GetCategoriesThatHaveContent), ex);
				throw new ApplicationException("Could not retrieve information about active categories with active content.", ex);
			}

			return categoriesThatHaveContent;
		}

		public async Task<List<UserCategory>> GetCategoriesToAddForUser(string[] categoriesSelected, string userId)
		{
			List<UserCategory> categoriesToAdd = new List<UserCategory>();

			foreach (var categoryId in categoriesSelected)
			{
				categoriesToAdd.Add(new UserCategory()
				{
					UserId = userId,
					CategoryId = int.Parse(categoryId)
				});
			}

			return await Task.Run(() => categoriesToAdd);
		}

		public async Task<List<UserCategory>> GetCategoriesToDeleteForUser(string userId)
		{
			var categoriesToDelete = await _repo.All<UserCategory>()
				.Where(uc => uc.UserId == userId)
				.Select(uc => new UserCategory()
				{
					Id = uc.Id,
					CategoryId = uc.CategoryId,
					UserId = uc.UserId
				})
				.ToListAsync();
			//var categoriesToDelete = await (from userCat in _context.UserCategories
			//								where userCat.UserId == userId
			//								select new UserCategory
			//								{
			//									Id = userCat.Id,
			//									CategoryId = userCat.CategoryId,
			//									UserId = userCat.UserId
			//								}).ToListAsync();
			return categoriesToDelete;
		}
	}
}
