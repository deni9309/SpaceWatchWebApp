
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceWatch.Infrastructure.Data.Entities
{
    public class Content
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 2)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "HTML/Text Content")]
        public string? HtmlContent { get; set; }

        [Display(Name = "Video Link")]
        public string? VideoLink { get; set; }

        [ForeignKey(nameof(CategoryItem))]
        public int CatItemId { get; set; }

        [Display(Name = "Media Item")]
        public CategoryItem CategoryItem { get; set; }

        [Display(Name = "Comments")]
        public virtual ICollection<UserComment> UserComments { get; set; }

        [NotMapped]
        public int CategoryId { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
