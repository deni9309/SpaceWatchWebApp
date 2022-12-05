using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Core.Models.DefaultArea
{
	public class CategoryDetailsViewModel
	{
		public IEnumerable<GroupedCategoryItemsByCategory> GroupedCategoryItemsByCategory { get; set; }
		public IEnumerable<CategoryViewModel> Categories { get; set; }

	}
}
