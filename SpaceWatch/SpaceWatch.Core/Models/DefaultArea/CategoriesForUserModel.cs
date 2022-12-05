using SpaceWatch.Infrastructure.Data.Entities;

namespace SpaceWatch.Core.Models.DefaultArea
{
	public class CategoriesForUserModel
	{
		public string UserId { get; set; }
		public ICollection<CategoryViewModel> Categories { get; set; }
		public ICollection<CategoryViewModel> CategoriesSelected { get; set; }
	}
}
