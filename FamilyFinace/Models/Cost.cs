using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Models
{
    public class Cost
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        public int CostCategoryId { get; set; }

        public CostCategory CostCategory { get; set; }
        
        [Required]
        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }

        [MaxLength(20)]
        public string Count { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [MaxLength(2000)]
        public string Comment { get; set; }

        [Required]
        public int PayTypeId { get; set; }
        public PayType PayType { get; set; }

        [Required]
        public int StoreId { get; set; }
        public Store Store { get; set; }
    }
}
