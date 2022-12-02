using SpaceWatch.Infrastructure.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SpaceWatch.Core.Models
{
	public class ContentViewModel
	{
		public int? Id { get; set; }

		[StringLength(200, MinimumLength = 2)]
		[Required]
		public string Title { get; set; } = null!;

		[Display(Name = "HTML/Text Content")]
		public string? HtmlContent { get; set; }

		[Display(Name = "Video Link")]
		public string? VideoLink { get; set; }

		public int CategoryId { get; set; }

		public string? CategoryName { get; set; } = null!;

		public int CatItemId { get; set; }

		//[Display(Name = "Media Item")]
		//public CategoryItemViewModel? CategoryItem { get; set; }

		//[Display(Name = "Comments")]
		//public IEnumerable<UserComment>? UserComments { get; set; }
	}
}
