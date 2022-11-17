﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpaceWatch.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SpaceWatch.Infrastructure.Data
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name = "First Name")]
        [StringLength(100, MinimumLength =2)]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(100, MinimumLength = 2)]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        [StringLength(300, MinimumLength = 2)]
        [Required]
        public string Address1 { get; set; }

        [Display(Name = "Second Address")]
        [StringLength(300, MinimumLength = 2)]
        public string? Address2 { get; set; }

        [Display(Name = "Post Code")]
        [Required]
        public string PostCode { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<UserCategory> UserCategories { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<UserComment> UserComments { get; set; }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryItem> CategoryItems { get; set; }
        public DbSet<MediaType> MediaTypes { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<UserCategory> UserCategories { get; set; }

        public DbSet<UserComment> UserComments { get; set; }
    }
}
