using SpaceWatch.Core.Models;

namespace SpaceWatch.Core.Contracts
{
	/// <summary>
	/// Provides methods that serve Content business logic for Admin role
	/// </summary>
	public interface IContentService
	{
		/// <summary>
		/// Gets all active content from database.
		/// </summary>
		/// <returns>IEnumerable of type ContentViewModel</returns>
		Task<IEnumerable<ContentViewModel>> GetAll();

		/// <summary>
		/// Adds new content to database. Method uses object of type ContentViewModel
		/// </summary>
		/// <param name="model"></param>
		/// <returns>id(int) of newly added content</returns>
		Task<int> Add(ContentViewModel model);

		/// <summary>
		/// Checks (by its id) whether active Content exists in database or not.
		/// </summary>
		/// <param name="ContentId"></param>
		/// <returns>true or false</returns>
		Task<bool> ContentExists(int ContentId);

		/// <summary>
		/// Gets Category details for specific content by content id
		/// </summary>
		/// <param name="contentId"></param>
		/// <returns>object of type CategoryViewModel</returns>
		Task<CategoryViewModel> GetCategoryForContentAsync(int contentId);

		/// <summary>
		/// Gets details about specific active content by its id.  
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Object of type ContentViewModel</returns>
		Task<ContentViewModel> ContentDetailsById(int id);

		/// <summary>
		/// Gets details about specific active content by CategoryItemId. 
		/// </summary>
		/// <param name="categoryItemId"></param>
		/// <returns>Object of type ContentViewModel</returns>
		Task<ContentViewModel> ContentDetailsByCategoryItemId(int categoryItemId);

		/// <summary>
		/// Updates Content by its id and ContentViewModel
		/// </summary>
		/// <param name="id"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		Task Edit(int id, ContentViewModel model);

		/// <summary>
		/// "Deletes" Content by setting IsActive property to "false". Method does not really remove the record from database.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task Delete(int id);
	}
}
