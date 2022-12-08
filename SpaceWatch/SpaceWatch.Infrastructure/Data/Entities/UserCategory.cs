using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceWatch.Infrastructure.Data.Entities
{
    /// <summary>
    /// Mapping table.
    /// Represents 'many-to-many' entity relation between User and Category entity.
    /// </summary>
    public class UserCategory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

		public ApplicationUser User { get; set; } = null!;

		[ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

		public Category Category { get; set; } = null!;
    }
}
