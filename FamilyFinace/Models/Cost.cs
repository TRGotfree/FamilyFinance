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
        public int Id { get; set; }
        
        [Required]
        public decimal Amount { get; set; }

        public string Count { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        public string Comment { get; set; }

        [Required]
        public int PayTypeId { get; set; }
        public PayType PayType { get; set; }

        [Required]
        public int StoreId { get; set; }
        public Store Store { get; set; }
    }
}
