﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Models
{
    public class CostSubCategory
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [Required]
        public bool IsRemoved { get; set; }
        
        [Required]
        public int CostCategoryId { get; set; }

        public CostCategory CostCategory { get; set; }
    }
}
