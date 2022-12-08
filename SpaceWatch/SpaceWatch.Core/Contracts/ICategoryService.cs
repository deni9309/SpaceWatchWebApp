using SpaceWatch.Core.Models;

namespace SpaceWatch.Core.Contracts
{
	/// <summary>
	/// Provides methods that serve Category business logic for Admin role
	/// </summary>
	public interface ICategoryService
	{
		/// <summary>
		/// Gets all active categories from database.
		/// </summary>
		/// <returns>IEnumerable of type CategoryViewModel</returns>
		Task<IEnumerable<CategoryViewModel>> GetAll();

		/// <summary>
		/// Adds new category to database. Method uses object of type CategoryViewModel
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task Add(CategoryViewModel model);

		/// <summary>
		/// Checks (by its id) whether active Category exists in database or not.
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns>true or false</returns>
		Task<bool> CategoryExists(int categoryId);

		/// <summary>
		/// Returns Category title by its id
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns>string CategoryTitle</returns>
		Task<string> GetCategoryTitleById(int categoryId);

		/// <summary>
		/// Returns detail information about Category by its id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Object of type CategoryViewModel</returns>
		Task<CategoryViewModel> CategoryDetailsById(int id);

		/// <summary>
		/// Updates Category by its id and CategoryViewModel
		/// </summary>
		/// <param name="categoryId"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		Task Edit(int categoryId, CategoryViewModel model);

		/// <summary>
		/// "Deletes" Category by setting IsActive property to "false". Method does not really remove the record from database.
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		Task Delete(int categoryId);

		/// <summary>
		/// "Deletes" sets IsActive=false on all releted entities that depend on this specific Category.
		/// Method does not really remove records from database.
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		Task DeleteReletedCategoryItemsAndContentFromCategory(int categoryId);	
	}
}
