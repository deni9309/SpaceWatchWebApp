using System.Collections.Generic;

namespace SpaceWatch.Core.Models
{
    public class UserCategoryListModel
    {
        public int CategoryId { get; set; }
        public ICollection<UserModel> Users { get; set; }
        public ICollection<UserModel> UsersSelected { get; set; }
    }
}
