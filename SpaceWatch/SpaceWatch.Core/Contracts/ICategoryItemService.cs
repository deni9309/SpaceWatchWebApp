using SpaceWatch.Core.Models;

namespace SpaceWatch.Core.Contracts
{
	/// <summary>
	/// Provides methods that serve CategoryItem business logic for Admin role
	/// </summary>
	public interface ICategoryItemService
	{
		/// <summary>
		/// Gets all active category items from specific category by its id.
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns>IEnumerable of CategoryItemViewModel</returns>
		Task<IEnumerable<CategoryItemViewModel>> GetAllCategoryItemsFromCategory(int categoryId);

		/// <summary>
		/// Adds new category item to database. Method uses object of type CategoryItemAddViewModel
		/// </summary>
		/// <param name="model"></param>
		/// <returns>Id (of type int) of the new category item</returns>
		Task<int> Add(CategoryItemAddViewModel model);

		/// <summary>
		/// Checks (by its id) whether active category item exists in database or not.
		/// </summary>
		/// <param name="categoryItemId"></param>
		/// <returns>true or false</returns>
		Task<bool> CategoryItemExists(int categoryItemId);

		/// <summary>
		/// Returns detail information about CategoryItem by its id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Object of type CategoryItemViewModel</returns>
		Task<CategoryItemViewModel> CategoryItemDetailsById(int id);

		/// <summary>
		/// Returns Category title by given CategoryItem id
		/// </summary>
		/// <param name="categoryItemId"></param>
		/// <returns>string CategoryTitle</returns>
		Task<string> GetCategoryTitleByCatItemId(int categoryItemId);

		/// <summary>
		/// Updates CategoryItem by its id and CategoryItemAddViewModel
		/// </summary>
		/// <param name="categoryItemId"></param>
		/// <param name="model"></param>
		/// <returns>Id(of type int) of the updated category item </returns>
		Task<int> Edit(int categoryItemId, CategoryItemAddViewModel model);

		/// <summary>
		/// "Deletes" categoryItem by setting IsActive property to "false". Method does not really remove the record from database.
		/// </summary>
		/// <param name="categoryItemId"></param>
		/// <returns></returns>
		Task Delete(int categoryItemId);

		/// <summary>
		/// "Deletes" sets IsActive=false on all releted entities that depend on this specific CategoryItem.
		/// Method does not really remove records from database.
		/// </summary>
		/// <param name="categoryItemId"></param>
		/// <returns></returns>
		Task DeleteReletedContent(int categoryItemId);
    }
}
