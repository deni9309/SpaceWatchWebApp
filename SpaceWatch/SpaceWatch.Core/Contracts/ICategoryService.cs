using SpaceWatch.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWatch.Core.Contracts
{
	public interface ICategoryService
	{
		Task<IEnumerable<CategoryViewModel>> GetAll();

		Task Add(CategoryViewModel model);

		Task<bool> CategoryExists(int categoryId);

		Task<string> GetCategoryTitleById(int categoryId);
		//Task<string> GetCatItemNameAsync(int contentId);

		Task<CategoryViewModel> CategoryDetailsById(int id);

		Task Edit(int categoryId, CategoryViewModel model);

		Task DeleteReletedCategoryItemsAndContentFromCategory(int categoryId);

        Task Delete(int categoryId);
	}
}
