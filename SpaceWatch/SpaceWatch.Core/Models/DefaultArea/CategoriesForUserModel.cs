
namespace SpaceWatch.Core.Models.DefaultArea
{
	public class CategoriesForUserModel
	{
		public string UserId { get; set; } = null!;

		public ICollection<CategoryViewModel> Categories { get; set; } = null!;

		public ICollection<CategoryViewModel> CategoriesSelected { get; set; } = null!;
	}
}
