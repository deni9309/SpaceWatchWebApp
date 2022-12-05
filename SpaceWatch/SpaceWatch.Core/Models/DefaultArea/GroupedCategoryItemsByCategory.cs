
namespace SpaceWatch.Core.Models.DefaultArea
{
	public class GroupedCategoryItemsByCategory
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public IGrouping<int, CategoryItemDetailsModel> Items { get; set; } = null!;
    }
}
