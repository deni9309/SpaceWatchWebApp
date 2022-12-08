using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceWatch.Infrastructure.Data.Entities
{
    /// <summary>
    /// An Item from specific category 
    /// </summary>
    public class CategoryItem
    {
		[NotMapped]
		private DateTime _releaseDate = DateTime.MinValue;

		[Key]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 2)]
        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(MediaType))]
        public int MediaTypeId { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> MediaTypes { get; set; } = null!;

        public DateTime DateTimeItemReleased
        {
            get { return _releaseDate == DateTime.MinValue ? DateTime.Now : _releaseDate; }
            set { _releaseDate = value;}
        }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
