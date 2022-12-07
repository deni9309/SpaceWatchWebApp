using System.ComponentModel.DataAnnotations;

namespace SpaceWatch.Core.Models.DefaultArea
{
    public class CommentViewModel
	{
        public int Id { get; set; }

        [StringLength(500)]
        [Required]
        public string CommentBody { get; set; } = null!;

        [Required]
        [Display(Name = "Posted on")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public int CategoryItemId { get; set; }
        public int ContentId { get; set; }
    }
}
