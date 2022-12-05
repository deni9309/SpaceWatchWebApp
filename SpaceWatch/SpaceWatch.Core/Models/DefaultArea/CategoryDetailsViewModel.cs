
namespace SpaceWatch.Core.Models.DefaultArea
{
	public class CategoryDetailsViewModel
	{
		public IEnumerable<GroupedCategoryItemsByCategory> GroupedCategoryItemsByCategory { get; set; } = null!;
        public IEnumerable<CategoryViewModel> Categories { get; set; } = null!;

    }
}
