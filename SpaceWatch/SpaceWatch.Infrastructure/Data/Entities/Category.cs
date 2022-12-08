using SpaceWatch.Infrastructure.Data.Extensions.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace SpaceWatch.Infrastructure.Data.Entities
{
    /// <summary>
    /// Represents a Genre for the media content (action, drama, horror...)
    /// </summary>
    public class Category : IPrimaryProperties
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Title { get; set; } = null!;

		public string? Description { get; set; }

        [Required]
        public string ThumbnailImagePath { get; set; } = null!;

        public virtual ICollection<CategoryItem> CategoryItems { get; set; } = null!;

        public virtual ICollection<UserCategory> UserCategories { get; set; } = null!;

		[Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
