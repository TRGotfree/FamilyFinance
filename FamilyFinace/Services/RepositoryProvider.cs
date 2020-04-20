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

        public async Task<CostSubCategory> AddCostSubCategory(CostSubCategory costSubCategory)
        {
            if (costSubCategory == null)
                throw new ArgumentNullException(nameof(costSubCategory));

            await repository.AddAsync(costSubCategory);
            await repository.SaveChangesAsync();

            return costSubCategory;
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

        public async Task<Store> AddStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            await repository.AddAsync(store);
            await repository.SaveChangesAsync();

            return store;
        }

        public async Task DeleteCost(Cost cost)
        {
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

        public async Task DeleteCostSubCategory(CostSubCategory costSubCategory)
        {
            if (costSubCategory == null)
                throw new ArgumentNullException(nameof(costSubCategory));


            repository.Remove(costSubCategory);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteIncome(Income income)
        {
            if (income == null)
                throw new ArgumentNullException(nameof(income));

            repository.Remove(income);
            await repository.SaveChangesAsync();
        }

        public async Task DeletePayType(PayType payType)
        {
            if (payType == null)
                throw new ArgumentNullException(nameof(payType));

            repository.Remove(payType);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            repository.Remove(store);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<CostCategory>> GetCostCategories()
        {
            return await repository.CostCategory.Where(c => !c.IsRemoved).ToListAsync();
        }

        public async Task<IEnumerable<Cost>> GetCosts(DateTime date)
        {
            return await repository.Cost.Where(c => c.Date == date).ToListAsync();
        }

        public async Task<IEnumerable<CostSubCategory>> GetCostSubCategories()
        {
            return await repository.CostSubCategory.Where(c => !c.IsRemoved).ToListAsync();
        }

        public async Task<IEnumerable<Income>> GetIncomes()
        {
            return await repository.Income.ToListAsync();
        }

        public async Task<IEnumerable<PayType>> GetPayTypes()
        {
            return await repository.PayType.Where(pt => !pt.IsRemoved).ToListAsync();
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

        public async Task<CostSubCategory> UpdateCostSubCategory(CostSubCategory costSubCategory)
        {
            if (costSubCategory == null)
                throw new ArgumentNullException(nameof(costSubCategory));

            repository.Update(costSubCategory);
            await repository.SaveChangesAsync();

            return costSubCategory;
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
