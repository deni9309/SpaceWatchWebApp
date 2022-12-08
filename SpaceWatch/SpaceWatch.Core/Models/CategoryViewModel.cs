using SpaceWatch.Infrastructure.Data.Extensions.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SpaceWatch.Core.Models
{
	public class CategoryViewModel : IPrimaryProperties
    {
		public int Id { get; set; }

		[StringLength(200, MinimumLength = 2)]
		[Required]
		public string Title { get; set; } = null!;

		public string? Description { get; set; }

		[Required]
		[Display(Name = "Thumbnail Image Path")]
		public string ThumbnailImagePath { get; set; } = null!;
	}
}
