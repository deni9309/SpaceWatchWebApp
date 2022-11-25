using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace SpaceWatch.Core.Models
{
	public class CategoryItemViewModel
	{
		private DateTime _releaseDate = DateTime.MinValue;

		public int Id { get; set; }

		[StringLength(200, MinimumLength = 2)]
		[Required]
		public string Title { get; set; }

		public string Description { get; set; }

		public int CategoryId { get; set; }

		[Required]
		public CategoryViewModel Category { get; set; } = null!;

		[Required(ErrorMessage = "Please select an item from the '{0}' dropdown list.")]
		[Display(Name = "Media Kind")]
		public int MediaTypeId { get; set; }

		[Display(Name = "Media Kind")]
		public IEnumerable<SelectListItem> MediaTypes { get; set; }

		[Display(Name = "Original Release Date")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime DateTimeItemReleased
		{
			get { return _releaseDate == DateTime.MinValue ? DateTime.Now : _releaseDate; }
			set { _releaseDate = value; }
		}

		public int ContentId { get; set; }
	}
}