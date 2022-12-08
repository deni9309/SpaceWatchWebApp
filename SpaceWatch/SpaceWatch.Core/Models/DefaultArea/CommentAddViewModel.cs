using System.ComponentModel.DataAnnotations;

namespace SpaceWatch.Core.Models.DefaultArea
{
    public class CommentAddViewModel
    {
        [MaxLength(500, ErrorMessage = "Please write your message up to 500 characters long!")]
        [Required]
        public string CommentBody { get; set; } = null!;

        public string? UserId { get; set; }

        public int CategoryItemId { get; set; }

        public int ContentId { get; set; }
    }
}
