using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.DTOModels
{
    public class Plan
    {
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Display(Name = "Категория")]
        public string CategoryName { get; set; }

        [Display(Name = "Подкатегория")]
        public string SubCategoryName { get; set; }

        [Display(Name = "Макс. значение затрат за прошлый период")]
        public string MaxFactAmount { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }

        [Display(Name = "Сумма по плану")]
        public string AmountToDisplay { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Range(2020, 2200)]
        public int Year { get; set; }


    }
}
