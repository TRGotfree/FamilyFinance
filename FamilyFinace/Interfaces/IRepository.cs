using FamilyFinace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FamilyFinace.Interfaces
{
    public interface IRepository
    {
        Task<List<Cost>> GetCosts(DateTime date);
        
        Task<Cost> AddCost(Cost cost);
        
        Task<Cost> UpdateCost(Cost cost);
        
        Task DeleteCost(int costId);

        Task<IEnumerable<CostCategory>> GetCostCategories();

        Task<CostCategory> AddCostCategory(CostCategory costCategory);

        Task<CostCategory> UpdateCostCategory(CostCategory costCategory);

        Task DeleteCostCategory(CostCategory costCategory);

        Task<IEnumerable<Store>> GetStores();

        Task<Store> AddStore(Store store);

        Task<Store> UpdateStore(Store store);

        Task DeleteStore(int storeId);

        Task<IEnumerable<PayType>> GetPayTypes();

        Task<PayType> AddPayType(PayType payType);

        Task<PayType> UpdatePayType(PayType payType);

        Task DeletePayType(int payTypeId);

        Task<List<Income>> GetIncomes(DateTime beginDate, DateTime endDate);

        Task<Income> AddIncome(Income income);

        Task<Income> UpdateIncome(Income income);

        Task DeleteIncome(int incomeId);

        Task<Plan> AddPlan(Plan plan);

        Task<IEnumerable<Plan>> GetPlans(int month, int year);

        Task<Plan> UpdatePlan(Plan plan);

        Task DeletePlan(int planId);

        Task<List<Plan>> GetPlansWithMaxAmountOfCosts(int month, int year);

        Task<Statistic> GetStatistic(int month, int year);
    }
}
