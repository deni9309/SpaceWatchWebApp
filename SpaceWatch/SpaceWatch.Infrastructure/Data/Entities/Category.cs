using SpaceWatch.Infrastructure.Data.Extensions.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace SpaceWatch.Infrastructure.Data.Entities
{
    public class Category : IPrimaryProperties
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 2)]
        [Required]
        public string Title { get; set; } = null!;

		public string? Description { get; set; }

        [Required]
        [Display(Name = "Thumbnail Image Path")]
        public string ThumbnailImagePath { get; set; } = null!;

        public virtual ICollection<CategoryItem> CategoryItems { get; set; }

        public virtual ICollection<UserCategory> UserCategories { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
