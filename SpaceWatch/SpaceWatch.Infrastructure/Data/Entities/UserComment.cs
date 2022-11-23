using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceWatch.Infrastructure.Data.Entities
{
    public class UserComment
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        [Required]
        public string CommentBody { get; set; }
        public DateTime DatePosted { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(Content))]
        public int ContentId { get; set; }
        public Content Content { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
