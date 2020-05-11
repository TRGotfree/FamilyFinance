using FamilyFinace.Interfaces;
using FamilyFinace.Models;
using FamilyFinace.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public async Task DeleteIncome(Income income)
        {
            if (income == null)
                throw new ArgumentNullException(nameof(income));

            repository.Remove(income);
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

        public async Task<IEnumerable<Cost>> GetCosts(DateTime date)
        {
            var costsCategories = await repository.CostCategory.ToListAsync();
            var costsOnCertainDate = await repository.Cost
                .Include(c => c.CostCategory)
                .Include(p => p.PayType)
                .Include(s => s.Store)
                .Where(c => c.Date == date).ToListAsync();

            var notExistingCostCategories = costsCategories.Where(c => !costsOnCertainDate.Select(cc => cc.CostCategoryId).Contains(c.Id)).ToList();

            for (int i = 0; i < notExistingCostCategories.Count; i++)
            {
                costsOnCertainDate.Add(new Cost
                {
                    CostCategoryId = notExistingCostCategories[i].Id,
                    Store = new Store(),
                    PayType = new PayType(),
                    CostCategory = notExistingCostCategories[i]
                });
            }

            return costsOnCertainDate.OrderBy(cc => cc.CostCategory.CategoryName);
        }

        public async Task<IEnumerable<Plan>> GetPlansWithMaxAmountOfCosts(int month, int year)
        {
            var costsCategories = await repository.CostCategory.ToListAsync();
            var averagePlanValues = await repository.Cost
                .Include(c => c.CostCategory)
                .GroupBy(ci => ci.CostCategory, cc => cc.Amount)
                .Join(repository.Plan, c => c.Key.Id, p => p.CategoryId, 
                (c, p) => new Plan { 
                    Id = p.Id, 
                    Month = p.Month, 
                    Year = p.Year, 
                    Amount = p.Amount, 
                    MaxAmountOfCost = c.Max(), 
                    CategoryId = c.Key.Id, 
                    Category = c.Key })
                .Where(p => p.Year == year && p.Month == month).ToListAsync();

            return averagePlanValues;
                //.Select(c => new Plan { Month = month, Year = year, Category = c.Key, CategoryId = c.Key.Id, MaxAmountOfCost = c.Max() }).ToListAsync();

            //var notExistingCostCategories = costsCategories.Where(c => !averagePlanValues.Select(cc => cc.CategoryId).Contains(c.Id)).ToList();
            //foreach (var costCategory in costsCategories)
            //{
            //    var plan = averagePlanValues.FirstOrDefault(p => p.CategoryId == costCategory.Id);
            //    if (plan != null)
            //    {

            //    }
            //    else
            //    {
            //        var newPlan = new Plan
            //        {
            //            Amount = 0, 
            //        }
            //    }
            //}

            //return costsOnCertainDate.OrderBy(cc => cc.CostCategory.CategoryName);
        }
        public async Task<IEnumerable<Income>> GetIncomes()
        {
            return await repository.Income.ToListAsync();
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
    }
}
