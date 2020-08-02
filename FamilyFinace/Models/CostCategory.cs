using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Models
{
    public class CostCategory
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string CategoryName { get; set; }

        [Required]
        [MaxLength(500)]
        public string SubCategoryName { get; set; }

        [Required]
        public bool IsRemoved { get; set; }
    }
}
