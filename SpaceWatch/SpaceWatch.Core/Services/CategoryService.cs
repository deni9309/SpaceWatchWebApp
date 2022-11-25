using Microsoft.EntityFrameworkCore;
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
	public class CategoryService : ICategoryService
	{
		private IRepository _repo;
		public CategoryService(IRepository repo)
		{
			_repo = repo;
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

		public async Task Delete(int categoryId)
		{
			var category = await _repo.GetByIdAsync<Category>(categoryId);
		
			category.IsActive = false;

			await _repo.SaveChangesAsync();
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
