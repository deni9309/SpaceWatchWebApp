using SpaceWatch.Core.Models;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Core.Contracts.DefaultArea
{
	public interface ICategoriesToUserService
	{
		/// <summary>
		/// Returns all active Categories that have content(also active) 
		/// </summary>
		/// <returns>ICollection of CategoryViewModel</returns>
		Task<ICollection<CategoryViewModel>> GetCategoriesThatHaveContent();

		/// <summary>
		///  Returns all active Categories that user is currently asigned to.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>ICollection of CategoryViewModel</returns>
		Task<ICollection<CategoryViewModel>> GetCategoriesCurrentlySavedForUser(string userId);
		
		/// <summary>
		/// Returns list of Categories that will be deleted from user's subscription list.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>List of UserCategory</returns>
		Task<List<UserCategory>> GetCategoriesToDeleteForUser(string userId);

		/// <summary>
		/// Returns list of Categories that will be added to user's subscription list.
		/// </summary>
		/// <param name="categoriesSelected"></param>
		/// <param name="userId"></param>
		/// <returns>List of UserCategory</returns>
		Task<List<UserCategory>> GetCategoriesToAddForUser(string[] categoriesSelected, string userId);
	}
}
