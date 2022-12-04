using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpaceWatch.Core.Models.DefaultArea
{
	public class DisplayContentForUserViewModel
	{
		public int Id { get; set; }

		public string Title { get; set; } = null!;

		[Display(Name = "HTML/Text Content")]
		public string? HtmlContent { get; set; }

		[Display(Name = "Video Link")]
		public string? VideoLink { get; set; }
	}
}
