using SpaceWatch.Infrastructure.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaceWatch.Infrastructure.Data
{
    public interface IDataFunctions
    {
        Task UpdateUserCategoryEntityAsync(List<UserCategory> userCategoryItemsToDelete, 
            List<UserCategory> userCategoryItemsToAdd);
    }
}
