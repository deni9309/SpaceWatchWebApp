using SpaceWatch.Infrastructure.Data.Entities;
using System.Collections.Generic;

namespace SpaceWatch.Models
{
    public class CategoriesToUserModel
    {
        public string UserId { get; set; }
        public ICollection<Category> Categories { get; set; }

        public ICollection<Category> CategoriesSelected { get; set; }
    }
}
