using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceWatch.Infrastructure.Data.Entities
{
    /// <summary>
    /// Represents user's comment (post) on specific Content entity
    /// </summary>
    public class Comment
    {
		[Key]
        public int Id { get; set; }

        [StringLength(500)]
        [Required]
        public string CommentBody { get; set; } = null!;

        public DateTime DatePosted { get; set; }

        public DateTime? DateEdited { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
		public ApplicationUser User { get; set; } = null!;

        [ForeignKey(nameof(Content))]
        public int ContentId { get; set; }
        public Content Content { get; set; } = null!;

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
