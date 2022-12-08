
using SpaceWatch.Infrastructure.Data.Extensions.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceWatch.Infrastructure.Data.Entities
{
    /// <summary>
    /// Represents a type of video (movie, tv series, trailer, HD video...)
    /// </summary>
    public class MediaType : IPrimaryProperties
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string ThumbnailImagePath { get; set; } = null!;

        [ForeignKey("MediaTypeId")]
        public IEnumerable<CategoryItem> CategoryItems { get; set; } = null!;

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
