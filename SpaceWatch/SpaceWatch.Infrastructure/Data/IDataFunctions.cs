using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Infrastructure.Data
{
    public interface IDataFunctions
    {
        Task UpdateUserCategoryEntityAsync(List<UserCategory> userCategoryItemsToDelete, 
            List<UserCategory> userCategoryItemsToAdd);
    }
}
