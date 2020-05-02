using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Models
{
    public class Plan
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Range(2020, 2200)]
        public int Year { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public CostCategory Category { get; set; }
    }
}
