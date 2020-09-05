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

            repository.Add(cost);
            await repository.SaveChangesAsync();

            return await repository.Cost
                .Include(c => c.CostCategory)
                .Include(c => c.PayType)
                .Include(c => c.Store)
                .FirstOrDefaultAsync(c => c.Id == cost.Id);
        }

        public async Task<CostCategory> AddCostCategory(CostCategory costCategory)
        {
            if (costCategory == null)
                throw new ArgumentNullException(nameof(costCategory));

            repository.Add(costCategory);
            await repository.SaveChangesAsync();

            return costCategory;
        }
        public async Task<Income> AddIncome(Income income)
        {
            if (income == null)
                throw new ArgumentNullException(nameof(income));

            repository.Add(income);
            await repository.SaveChangesAsync();

            return income;
        }

        public async Task<PayType> AddPayType(PayType payType)
        {
            if (payType == null)
                throw new ArgumentNullException(nameof(payType));

            repository.Add(payType);
            await repository.SaveChangesAsync();

            return payType;
        }

        public async Task<Plan> AddPlan(Plan plan)
        {
            if (plan == null)
                throw new ArgumentNullException(nameof(plan));

            repository.Plan.Add(plan);

            await repository.SaveChangesAsync();

            return await repository.Plan.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == plan.Id);
        }

        public async Task<Store> AddStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            repository.Add(store);
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

        public async Task DeleteCostCategory(int costCategoryId)
        {
            var categoryToRemove = await repository.CostCategory.FindAsync(costCategoryId);
            if (categoryToRemove == null)
                return;

            repository.Remove(categoryToRemove);
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

        public Task<List<CostCategory>> GetCostCategories()
        {
            return repository.CostCategory.Where(c => !c.IsRemoved).ToListAsync();
        }

        public Task<List<Cost>> GetCosts(DateTime date)
        {
            var costsCategoriesQuery = repository.CostCategory;
            var costsOnCertainDateQuery = repository.Cost.Include(s => s.Store)
                .Include(p => p.PayType)
                .Include(c => c.CostCategory).Where(c => c.Date == date);

            var resultQuery = from costCategorie in costsCategoriesQuery
                              join cd in costsOnCertainDateQuery on costCategorie.Id equals cd.CostCategoryId into c
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

            return resultQuery.OrderByDescending(cc => cc.Amount).ToListAsync();
        }

        public Task<List<Plan>> GetPlansWithMaxAmountOfCosts(int month, int year)
        {
            var costCategories = repository.CostCategory.Where(c => !c.IsRemoved);
            var costPlans = repository.Plan.Include(p => p.Category).Where(p => p.Month == month && p.Year == year);
            var maxCosts = repository.Cost.GroupBy(c => new { c.CostCategoryId, c.Date.Month })
                                          .Select(cc => new Cost { Amount = cc.Sum(c => c.Amount), CostCategoryId = cc.Key.CostCategoryId })
                                          .GroupBy(cc => cc.CostCategoryId)
                                          .Select(cc => new Cost { Amount = cc.Max(c => c.Amount), CostCategoryId = cc.Key });

            var plansQuery = from categorie in costCategories
                             join p in costPlans on categorie.Id equals p.CategoryId into pl
                             from plan in pl.DefaultIfEmpty()
                             join mc in maxCosts on categorie.Id equals mc.CostCategoryId into cc
                             from cost in cc.DefaultIfEmpty()
                             select new Plan { Id = plan.Id, Amount = plan.Amount, Category = categorie, CategoryId = categorie.Id, Month = month, Year = year, MaxAmountOfCost = cost.Amount };

            return plansQuery.OrderByDescending(p => p.Amount).ToListAsync();
        }
        public Task<List<Income>> GetIncomes(DateTime beginDate, DateTime endDate)
        {
            return repository.Income.Include(i => i.PayType).Where(i => i.Date >= beginDate && i.Date <= endDate).ToListAsync();
        }

        public async Task<List<PayType>> GetPayTypes()
        {
            return await repository.PayType.Where(pt => !pt.IsRemoved).ToListAsync();
        }

        public async Task<List<Store>> GetStores()
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
            return await repository.Plan.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == plan.Id);
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
