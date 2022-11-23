
using SpaceWatch.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;
using System;

namespace SpaceWatch.Core.Models
{
	public class CommentViewModel
	{
		public int Id { get; set; }

		[StringLength(500)]
		[Required]
		public string CommentBody { get; set; }

		[Required]
		[Display(Name = "Posted on")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy mm:hh}")]  
	    public DateTime DatePosted { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public int ContentId { get; set; }
		public ContentViewModel Content { get; set; }
	}
}
