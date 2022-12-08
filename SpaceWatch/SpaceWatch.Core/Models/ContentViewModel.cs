using System.ComponentModel.DataAnnotations;

namespace SpaceWatch.Core.Models
{
	public class ContentViewModel
	{
		public int? Id { get; set; }

		[StringLength(200, MinimumLength = 2)]
		[Required]
		public string Title { get; set; } = null!;

		[Display(Name = "Text Content")]
		public string? HtmlContent { get; set; }

		[Display(Name = "Video Link")]
		public string? VideoLink { get; set; }

		public int CategoryId { get; set; }

		public string? CategoryName { get; set; } = null!;

		public int CatItemId { get; set; }
	}
}
