using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.DTOModels
{
    public class Cost
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
       
        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
        public string Category { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CostSubCategoryId { get; set; }
        public string CostSubCategory { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public decimal Amount { get; set; }
        public string Count { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Comment { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int PayTypeId { get; set; }
        public string PayType { get; set; }
        public string Store { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int StoreId { get; set; }
    }
}
}
