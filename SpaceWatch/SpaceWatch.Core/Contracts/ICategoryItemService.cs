using SpaceWatch.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWatch.Core.Contracts
{
	public interface ICategoryItemService
	{
		Task<IEnumerable<CategoryItemViewModel>> GetAllCategoryItemsFromCategory(int categoryId);

        Task<int> Add(CategoryItemAddViewModel model);

		Task<bool> CategoryItemExists(int categoryItemId);

		Task<CategoryItemViewModel> CategoryItemDetailsById(int id);

		Task<string> GetCategoryTitleByCatItemId(int categoryItemId);

		Task<int> Edit(int categoryItemId, CategoryItemAddViewModel model);

		Task Delete(int categoryItemId);
	}
}
