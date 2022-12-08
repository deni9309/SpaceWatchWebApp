using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceWatch.Infrastructure.Data.Entities
{
    /// <summary>
    /// Represents a media (video)
    /// </summary>
    public class Content
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 2)]
        [Required]
        public string Title { get; set; } = null!;

        public string? HtmlContent { get; set; }

        public string? VideoLink { get; set; }

        [ForeignKey(nameof(CategoryItem))]
        public int CatItemId { get; set; }

        public CategoryItem CategoryItem { get; set; } = null!;

        public IEnumerable<Comment>? UserComments { get; set; }

        [NotMapped]
        public int CategoryId { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
