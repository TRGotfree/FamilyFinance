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

        [Display(Name = "Категория")]
        public string Category { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CostSubCategoryId { get; set; }

        [Display(Name = "Подкатегория")]
        public string CostSubCategory { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Сумма")]
        public decimal Amount { get; set; }

        [Display(Name = "Кол-во")]
        public string Count { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Comment { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PayTypeId { get; set; }
        [Display(Name = "Тип оплаты")]
        public string PayType { get; set; }

        [Display(Name = "Магазин")]
        public string Store { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int StoreId { get; set; }
    }

}
