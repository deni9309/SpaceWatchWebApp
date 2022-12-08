
namespace SpaceWatch.Core.Contracts.DefaultArea
{
	/// <summary>
	/// Provides methods that serve User's authorization and authentication functionality.
	/// </summary>
	public interface IUserAuthService
	{
		/// <summary>
		/// Checks whether User with specific username is already in database or not.
		/// </summary>
		/// <param name="userName"></param>
		/// <returns>true or false</returns>
		Task<bool> UserNameExists(string userName);

		/// <summary>
		/// (For newly registerd users.) Assigns an User to specific Category.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="categoryId"></param>
		/// <returns></returns>
		Task AddCategoryToNewUserAsync(string userId, int categoryId);
	}
}
