using SpaceWatch.Core.Models;
using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Core.Contracts
{
	/// <summary>
	/// Provides methods that serve UserCategory relations and business logic for Admin role.
	/// </summary>
	public interface IUsersToCategoryService
    {
		/// <summary>
		/// Gets all registered Users in database
		/// </summary>
		/// <returns>List of type UserModel</returns>
		Task<List<UserModel>> GetAllUsers();

		/// <summary>
		/// Gets a collection of users who will be assigned to specific category
		/// </summary>
		/// <param name="userCategoryListModel"></param>
		/// <returns>List of type UserCategory</returns>
		Task<List<UserCategory>> GetUsersSelectedForCategoryToAdd(UserCategoryListModel userCategoryListModel);

		/// <summary>
		/// Gets a collection of users who will be removed from UserCategory table (unassigned from specific category).
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns>List of type UserCategory</returns>
		Task<List<UserCategory>> GetUsersSelectedForCategoryToDelete(int categoryId);

		/// <summary>
		/// Adds or/and removes records from UserCategories table. 
		/// </summary>
		/// <param name="userCategoryListModel"></param>
		/// <returns></returns>
        Task UpdateUsersSubscriptionsForCategory(UserCategoryListModel userCategoryListModel);

		/// <summary>
		/// Gets a collection of current users assigned to specific category by its id.
		/// </summary>
		/// <param name="categoryId"></param>
		/// <returns>List of type UserModel</returns>
		Task<List<UserModel>> GetSavedSelectedUsersForCategory(int categoryId);
    }
}
