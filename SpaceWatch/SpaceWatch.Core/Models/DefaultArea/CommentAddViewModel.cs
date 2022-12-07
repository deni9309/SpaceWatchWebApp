using System.ComponentModel.DataAnnotations;

namespace SpaceWatch.Core.Models.DefaultArea
{
    public class CommentAddViewModel
    {
        [MaxLength(500, ErrorMessage = "Please write your message up to 500 characters long!")]
        [Required]
        public string CommentBody { get; set; } = null!;

        //[Display(Name = "Posted on")]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy mm:hh}")]
        //public DateTime? DatePosted { get; set; }

        public string? UserId { get; set; }

        public int CategoryItemId { get; set; }

        public int ContentId { get; set; }
    }
}
