
namespace SpaceWatch.Core.Models.DefaultArea
{
	public class GroupedCategoryItemsByCategory
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public IGrouping<int, CategoryItemDetailsModel> Items { get; set; }
	}
}
