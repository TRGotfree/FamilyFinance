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

        Task<List<Cost>> GetCosts(DateTime beginDate, DateTime endDate);
        
        Task<Cost> AddCost(Cost cost);
        
        Task<Cost> UpdateCost(Cost cost);
        
        Task DeleteCost(int costId);

        Task<List<CostCategory>> GetCostCategories();

        Task<CostCategory> AddCostCategory(CostCategory costCategory);

        Task<CostCategory> UpdateCostCategory(CostCategory costCategory);

        Task DeleteCostCategory(int costCategoryId);

        Task<List<Store>> GetStores();

        Task<Store> AddStore(Store store);

        Task<Store> UpdateStore(Store store);

        Task DeleteStore(int storeId);

        Task<List<PayType>> GetPayTypes();

        Task<PayType> AddPayType(PayType payType);

        Task<PayType> UpdatePayType(PayType payType);

        Task DeletePayType(int payTypeId);

        Task<List<Income>> GetIncomes(DateTime beginDate, DateTime endDate);

        Task<Income> AddIncome(Income income);

        Task<Income> UpdateIncome(Income income);

        Task DeleteIncome(int incomeId);

        Task<Plan> AddPlan(Plan plan);

        Task<Plan> UpdatePlan(Plan plan);

        Task DeletePlan(int planId);

        Task<List<Plan>> GetPlansWithMaxAmountOfCosts(int month, int year);

        Task<Statistic> GetStatistic(int month, int year);

        //Task<>
    }
}
