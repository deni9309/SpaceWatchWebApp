using SpaceWatch.Infrastructure.Data.Extensions.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpaceWatch.Core.Models
{
	public class MediaTypeViewModel : IPrimaryProperties
	{
		public int Id { get; set; }

		[StringLength(200, MinimumLength = 2)]
		[Required]
		public string Title { get; set; } = null!;

		[Required]
		[Display(Name = "Thumbnail Image Path")]
		public string ThumbnailImagePath { get; set; } = null!;

		//public IEnumerable<CategoryItemViewModel>? CategoryItems { get; set; } = new List<CategoryItemViewModel>();
	}
}
