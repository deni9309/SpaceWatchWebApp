
namespace SpaceWatch.Core.Models.DefaultArea
{
	public class CategoryItemDetailsModel
	{
		public int CategoryId { get; set; }
		public string CategoryTitle { get; set; } = null!;
		public int CategoryItemId { get; set; }
		public string CategoryItemTitle { get; set; } = null!;
		public string? CategoryItemDescription { get; set; }
		public string MediaImagePath { get; set; } = null!;
	}
}
