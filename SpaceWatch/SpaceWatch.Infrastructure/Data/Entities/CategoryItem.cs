﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceWatch.Infrastructure.Data.Entities
{
    public class CategoryItem
    {    
        [Key]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 2)]
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Please select a valid item from the '{0}' dropdown list.")]
        [Display(Name = "Media Kind")]
        [ForeignKey(nameof(MediaType))]
        public int MediaTypeId { get; set; }

        [NotMapped]
        [Display(Name = "Media Kind")]
        public IEnumerable<SelectListItem> MediaTypes { get; set; }

        [Display(Name = "Original Release Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateTimeItemReleased
        {
            get { return _releaseDate == DateTime.MinValue ? DateTime.Now : _releaseDate; }
            set { _releaseDate = value;}
        }

        [NotMapped]
        public int ContentId { get; set; }
        [NotMapped]

        private DateTime _releaseDate = DateTime.MinValue;

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}