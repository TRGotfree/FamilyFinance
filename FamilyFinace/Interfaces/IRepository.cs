using FamilyFinace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FamilyFinace.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<Cost>> GetCosts(DateTime date);
        
        Task<Cost> AddCost(Cost cost);
        
        Task<Cost> UpdateCost(Cost cost);
        
        Task DeleteCost(Cost cost);

        Task<IEnumerable<CostCategory>> GetCostCategories();

        Task<CostCategory> AddCostCategory(CostCategory costCategory);

        Task<CostCategory> UpdateCostCategory(CostCategory costCategory);

        Task DeleteCostCategory(CostCategory costCategory);

        Task<IEnumerable<CostSubCategory>> GetCostSubCategories();

        Task<CostSubCategory> AddCostSubCategory(CostSubCategory costSubCategory);

        Task<CostSubCategory> UpdateCostSubCategory(CostSubCategory costSubCategory);

        Task DeleteCostSubCategory(CostSubCategory costSubCategory);

        Task<IEnumerable<Store>> GetStores();

        Task<Store> AddStore(Store store);

        Task<Store> UpdateStore(Store store);

        Task DeleteStore(Store store);

        Task<IEnumerable<PayType>> GetPayTypes();

        Task<PayType> AddPayType(PayType payType);

        Task<PayType> UpdatePayType(PayType payType);

        Task DeletePayType(PayType payType);

        Task<IEnumerable<Income>> GetIncomes();

        Task<Income> AddIncome(Income income);

        Task<Income> UpdateIncome(Income income);

        Task DeleteIncome(Income income);

    }
}
