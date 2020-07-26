using FamilyFinace.Interfaces;
using FamilyFinace.Models;
using FamilyFinace.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.Versioning;
using System.Threading.Tasks;


namespace FamilyFinace.Services
{
    public class RepositoryProvider : IRepository
    {
        private readonly RepositoryContext repository;

        public RepositoryProvider(RepositoryContext repositoryContext)
        {
            this.repository = repositoryContext ?? throw new ArgumentNullException(nameof(repositoryContext));
        }
        public async Task<Cost> AddCost(Cost cost)
        {
            if (cost == null)
                throw new ArgumentNullException(nameof(cost));

            await repository.AddAsync(cost);
            await repository.SaveChangesAsync();

            return cost;
        }

        public async Task<CostCategory> AddCostCategory(CostCategory costCategory)
        {
            if (costCategory == null)
                throw new ArgumentNullException(nameof(costCategory));

            await repository.AddAsync(costCategory);
            await repository.SaveChangesAsync();

            return costCategory;
        }
        public async Task<Income> AddIncome(Income income)
        {
            if (income == null)
                throw new ArgumentNullException(nameof(income));

            await repository.AddAsync(income);
            await repository.SaveChangesAsync();
            
            return income;
        }

        public async Task<PayType> AddPayType(PayType payType)
        {
            if (payType == null)
                throw new ArgumentNullException(nameof(payType));

            await repository.AddAsync(payType);
            await repository.SaveChangesAsync();

            return payType;
        }

        public async Task<Plan> AddPlan(Plan plan)
        {
            if (plan == null)
                throw new ArgumentNullException(nameof(plan));

            await repository.AddAsync(plan);
            await repository.SaveChangesAsync();

            return plan;
        }

        public async Task<Store> AddStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            await repository.AddAsync(store);
            await repository.SaveChangesAsync();

            return store;
        }

        public async Task DeleteCost(int costId)
        {
            var cost = repository.Cost.FirstOrDefault(c => c.Id == costId);
            if (cost == null)
                throw new ArgumentNullException(nameof(cost));

            repository.Remove(cost);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteCostCategory(CostCategory costCategory)
        {
            if (costCategory == null)
                throw new ArgumentNullException(nameof(costCategory));

            repository.Remove(costCategory);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteIncome(int incomeId)
        {
            var incomeToDelete = await repository.Income.FirstOrDefaultAsync(i => i.Id == incomeId);
            repository.Remove(incomeToDelete);
            await repository.SaveChangesAsync();
        }

        public async Task DeletePayType(int payTypeId)
        {
            var payType = await repository.PayType.FirstOrDefaultAsync(p => p.Id == payTypeId);

            if (payType == null)
                throw new ArgumentNullException(nameof(payType));

            payType.IsRemoved = true;

            repository.Update(payType);

            await repository.SaveChangesAsync();
        }

        public async Task DeletePlan(int planId)
        {
            var plan = await repository.Plan.FirstOrDefaultAsync(p => p.Id == planId);
            if (plan == null)
                throw new ArgumentNullException(nameof(plan));

            repository.Remove(plan);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteStore(int storeId)
        {
            var store = await repository.Store.FirstOrDefaultAsync(s => s.Id == storeId);

            if (store == null)
                throw new ArgumentNullException(nameof(store));

            store.IsRemoved = true;

            repository.Update(store);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<CostCategory>> GetCostCategories()
        {
            return await repository.CostCategory.Where(c => !c.IsRemoved).ToListAsync();
        }

        public Task<List<Cost>> GetCosts(DateTime date)
        {
            var costsCategoriesQuery = repository.CostCategory;
            var costsOnCertainDateQuery = repository.Cost.Include(s => s.Store)
                .Include(p => p.PayType)
                .Include(c => c.CostCategory).Where(c => c.Date == date);

            var resultQuery = from costCategorie in costsCategoriesQuery
                              join cd in costsOnCertainDateQuery.DefaultIfEmpty() on costCategorie.Id equals cd.CostCategoryId into c
                              from cost in c.DefaultIfEmpty()
                              select new Cost
                              {
                                  Amount = cost.Amount,
                                  Comment = cost.Comment,
                                  CostCategoryId = costCategorie.Id,
                                  CostCategory = costCategorie,
                                  Id = cost.Id,
                                  Count = cost.Count,
                                  Date = date,
                                  PayTypeId = cost.PayTypeId,
                                  PayType = cost.PayType,
                                  StoreId = cost.PayTypeId,
                                  Store = cost.Store
                              };

            return resultQuery.OrderBy(cc => cc.CostCategory.CategoryName).ToListAsync();
        }

        public Task<List<Plan>> GetPlansWithMaxAmountOfCosts(int month, int year)
        {
            var costCategories = repository.CostCategory.Where(c => !c.IsRemoved);
            var costPlans = repository.Plan.Include(p => p.Category).Where(p => p.Month == month && p.Year == year);
            var maxCosts = repository.Cost.GroupBy(c => c.CostCategoryId ).Select(cc => new Cost { Amount = cc.Max(c => c.Amount), CostCategoryId = cc.Key });

            var plansQuery = from categorie in costCategories
                             join p in costPlans on categorie.Id equals p.CategoryId into pl
                             from plan in pl.DefaultIfEmpty()
                             join mc in maxCosts on categorie.Id equals mc.CostCategoryId into cc
                             from cost in cc.DefaultIfEmpty()
                             select new Plan { Id = plan.Id, Amount = plan.Amount, Category = categorie, CategoryId = categorie.Id, Month = month, Year = year, MaxAmountOfCost = cost.Amount };

            return plansQuery.OrderBy(p => p.Category.CategoryName).ToListAsync();
        }
        public Task<List<Income>> GetIncomes(DateTime beginDate, DateTime endDate)
        {
            return repository.Income.Include(i => i.PayType).Where(i => i.Date >= beginDate && i.Date <= endDate).ToListAsync();
        }

        public async Task<IEnumerable<PayType>> GetPayTypes()
        {
            return await repository.PayType.Where(pt => !pt.IsRemoved).ToListAsync();
        }

        public async Task<IEnumerable<Plan>> GetPlans(int month, int year)
        {
            var costsCategories = await repository.CostCategory.ToListAsync();
            var plans = await repository.Plan.Include(c => c.Category).Where(p => p.Month == month && p.Year == year).ToListAsync();

            var notExistingCostCategories = costsCategories.Where(c => !plans.Select(p => p.CategoryId).Contains(c.Id)).ToList();

            for (int i = 0; i < notExistingCostCategories.Count; i++)
            {
                plans.Add(new Plan
                {
                    CategoryId = notExistingCostCategories[i].Id,
                    Category = notExistingCostCategories[i],
                    Month = month,
                    Year = year,
                    Amount = 0
                });
            }

            return plans.OrderBy(cc => cc.Category.CategoryName);
        }

        public async Task<IEnumerable<Store>> GetStores()
        {
            return await repository.Store.Where(s => !s.IsRemoved).ToListAsync();
        }

        public async Task<Cost> UpdateCost(Cost cost)
        {
            if (cost == null)
                throw new ArgumentNullException(nameof(cost));

            repository.Update(cost);
            await repository.SaveChangesAsync();

            return cost;
        }

        public async Task<CostCategory> UpdateCostCategory(CostCategory costCategory)
        {
            if (costCategory == null)
                throw new ArgumentNullException(nameof(costCategory));

            repository.Update(costCategory);
            await repository.SaveChangesAsync();

            return costCategory;
        }

        public async Task<Income> UpdateIncome(Income income)
        {
            if (income == null)
                throw new ArgumentNullException(nameof(income));

            repository.Update(income);
            await repository.SaveChangesAsync();

            return income;
        }

        public async Task<PayType> UpdatePayType(PayType payType)
        {
            if (payType == null)
                throw new ArgumentNullException(nameof(payType));

            repository.Update(payType);
            await repository.SaveChangesAsync();

            return payType;
        }

        public async Task<Plan> UpdatePlan(Plan plan)
        {
            if (plan == null)
                throw new ArgumentNullException(nameof(plan));

            repository.Update(plan);
            await repository.SaveChangesAsync();
            return plan;
        }

        public async Task<Store> UpdateStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            repository.Update(store);
            await repository.SaveChangesAsync();

            return store;
        }

        public async Task<Statistic> GetStatistic(int month, int year)
        {
            Statistic statistic = new Statistic();
            statistic.CostTotal = await repository.Cost.Where(c => c.Date.Month == month && c.Date.Year == year).SumAsync(c => c.Amount);
            statistic.PlanTotal = await repository.Plan.Where(p => p.Month == month && p.Year == year).SumAsync(p => p.Amount);
            statistic.IncomeTotal = await repository.Income.Where(i => i.Date.Month == month && i.Date.Year == year).SumAsync(i => i.Amount);

            return statistic;
        }
    }
}
