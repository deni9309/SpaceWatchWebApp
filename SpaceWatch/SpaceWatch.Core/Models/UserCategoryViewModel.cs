using SpaceWatch.Infrastructure.Data.Entities;
using SpaceWatch.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWatch.Core.Models
{
    public class UserCategoryViewModel
    {

        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
 
        public int CategoryId { get; set; }

        public Category Category { get; set; }

    }
}
