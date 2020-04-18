using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Models
{
    public class Income
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int PayTypeId { get; set; }
        public PayType PayType { get; set; }
        public string Comment { get; set; }
    }
}
