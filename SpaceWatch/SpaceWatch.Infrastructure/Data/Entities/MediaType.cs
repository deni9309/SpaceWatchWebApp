
using SpaceWatch.Infrastructure.Data.Extensions.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceWatch.Infrastructure.Data.Entities
{
    public class MediaType : IPrimaryProperties
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 2)]
        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Thumbnail Image Path")]
        public string ThumbnailImagePath { get; set; }

        [ForeignKey("MediaTypeId")]
        public IEnumerable<CategoryItem> CategoryItems { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
