using SpaceWatch.Core.Models.DefaultArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWatch.Core.Contracts.DefaultArea
{
    public interface IHomeService
    {
        Task<IEnumerable<CategoryItemDetailsModel>> GetCategoryItemDetailsForUser(string userId);

        Task<IEnumerable<GroupedCategoryItemsByCategory>> GetGroupedCategoryItemsByCategory(IEnumerable<CategoryItemDetailsModel> catItemModels);



    }
}
