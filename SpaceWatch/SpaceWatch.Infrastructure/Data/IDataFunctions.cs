using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Infrastructure.Data
{
    /// <summary>
    /// Interface for complex data manipulations
    /// </summary>
    public interface IDataFunctions
    {
		/// <summary>
		/// Updates (adds or/and deletes) records from UserCategory entity by performing database transaction
		/// </summary>
		/// <param name="userCategoryItemsToDelete"></param>
		/// <param name="userCategoryItemsToAdd"></param>
		/// <returns></returns>
		Task UpdateUserCategoryEntityAsync(List<UserCategory> userCategoryItemsToDelete, 
            List<UserCategory> userCategoryItemsToAdd);
    }
}
