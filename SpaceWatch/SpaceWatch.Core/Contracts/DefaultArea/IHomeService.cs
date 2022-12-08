using SpaceWatch.Core.Models.DefaultArea;

namespace SpaceWatch.Core.Contracts.DefaultArea
{
	/// <summary>
	/// Provides methods that serve Home page functionality and layout specifications
	/// </summary>
	public interface IHomeService
    {
		/// <summary>
		/// Gets detail information for each active category item user is assigned to.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>IEnumerable of CategoryItemDetailsModel</returns>
		Task<IEnumerable<CategoryItemDetailsModel>> GetCategoryItemDetailsForUser(string userId);

		/// <summary>
		/// Groups collection of category items by Category.
		/// </summary>
		/// <param name="catItemModels"></param>
		/// <returns>IEnumerable of GroupedCategoryItemsByCategory</returns>
		Task<IEnumerable<GroupedCategoryItemsByCategory>> GetGroupedCategoryItemsByCategory(IEnumerable<CategoryItemDetailsModel> catItemModels);
    }
}
