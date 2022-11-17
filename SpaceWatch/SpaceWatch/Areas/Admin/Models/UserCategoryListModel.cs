using System.Collections.Generic;


namespace SpaceWatch.Areas.Admin.Models
{
    public class UserCategoryListModel
    {
        public int CategoryId { get; set; }
        public ICollection<UserModel> Users { get; set; }
        public ICollection<UserModel> UsersSelected { get; set; }
    }
}
