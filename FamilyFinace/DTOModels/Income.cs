﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.DTOModels
{
    public class Income
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
        
        [Required]
        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        [Display(Name = "Сумма")]
        public string AmountToDisplay { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PayTypeId { get; set; }

        [Display(Name = "Тип дохода")]
        public string PayType { get; set; }
        
        [MaxLength(2000)]
        public string Comment { get; set; }

        [Display(Name = "Комментарий")]
        public string ShortComment { get; set; }
    }
}
