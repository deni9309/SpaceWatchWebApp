using SpaceWatch.Infrastructure.Data.Extensions.Interfaces;

namespace SpaceWatch.Core.Models
{
	public class MediaTypeSelectListModel : IPrimaryProperties
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
	}
}
