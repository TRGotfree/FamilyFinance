using FamilyFinace.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyFinace.Services
{
    public class ModelTransformer : IModelTransformer
    {
        public Models.Cost FromDTOModelCostToModelCost(DTOModels.Cost dtoModelCost)
        {
            if (dtoModelCost == null)
                throw new ArgumentNullException(nameof(dtoModelCost), "Couldn't transform DTOModels.Cost to Models.Cost object! Input parameter is null!");

            Models.Cost costModel = new Models.Cost
            {
                Amount = dtoModelCost.Amount,
                Comment = dtoModelCost.Comment,
                CostCategoryId = dtoModelCost.CategoryId,
                PayTypeId = dtoModelCost.PayTypeId,
                StoreId = dtoModelCost.StoreId,
                Id = dtoModelCost.Id,
                Count = dtoModelCost.Count,
                Date = dtoModelCost.Date
            };

            return costModel;
        }

        public DTOModels.Cost FromModelsCostToDTOModelCost(Models.Cost modelCost)
        {
            if (modelCost == null)
                throw new ArgumentNullException(nameof(modelCost), "Couldn't transform Models.Cost to DTOModels.Cost object! Input parameter is null!");

            DTOModels.Cost dtoModelCost = new DTOModels.Cost
            {
                Amount = modelCost.Amount,
                Category = modelCost.CostCategory.CategoryName,
                CategoryId = modelCost.CostCategoryId,
                CostSubCategory = modelCost.CostCategory.SubCategoryName,
                Comment = modelCost.Comment,
                Count = modelCost.Count,
                Date = modelCost.Date,
                Id = modelCost.Id,
                PayType = modelCost.PayType.Name,
                PayTypeId = modelCost.PayTypeId,
                Store = modelCost.Store.Name,
                StoreId = modelCost.StoreId
            };

            return dtoModelCost;
        }

        public List<Models.Cost> RangeOfDTOModelCostsToRangeOfModelCosts(IEnumerable<DTOModels.Cost> dtoModelsCosts)
        {
            if (dtoModelsCosts == null)
                throw new ArgumentNullException(nameof(dtoModelsCosts));

            List<Models.Cost> costs = new List<Models.Cost>(0);

            for (int i = 0; i < dtoModelsCosts.Count(); i++)
                costs.Add(FromDTOModelCostToModelCost(dtoModelsCosts.ElementAt(i)));

            return costs;
        }

        public List<DTOModels.Cost> RangeOfModelCostsToRangeOfDTOModelCosts(IEnumerable<Models.Cost> modelsCosts)
        {
            if (modelsCosts == null)
                throw new ArgumentNullException(nameof(modelsCosts));

            List<DTOModels.Cost> costs = new List<DTOModels.Cost>(0);

            for (int i = 0; i < modelsCosts.Count(); i++)
                costs.Add(FromModelsCostToDTOModelCost(modelsCosts.ElementAt(i)));

            return costs;
        }

        public Models.Plan FromDTOModelPlanToModelPlan(DTOModels.Plan dtoPlan)
        {
            if (dtoPlan == null)
                throw new ArgumentNullException(nameof(dtoPlan));

            return new Models.Plan()
            {
                Amount = dtoPlan.Amount,
                CategoryId = dtoPlan.CategoryId,
                Id = dtoPlan.Id,
                Month = dtoPlan.Month,
                Year = dtoPlan.Year
            };
        }

        public DTOModels.Plan FromModelPlanToDTOModelPlan(Models.Plan modelPlan)
        {
            if (modelPlan == null)
                throw new ArgumentNullException(nameof(modelPlan));

            return new DTOModels.Plan()
            {
                Amount = modelPlan.Amount,
                MaxFactAmount = modelPlan.MaxAmountOfCost,
                CategoryId = modelPlan.CategoryId,
                CategoryName = modelPlan.Category?.CategoryName,
                SubCategoryName = modelPlan.Category?.SubCategoryName,
                Id = modelPlan.Id,
                Month = modelPlan.Month,
                Year = modelPlan.Year
            };
        }

        public List<Models.Plan> RangeOfDTOModelsPlanToRangeOfModelPlans(IEnumerable<DTOModels.Plan> dtoPlans)
        {
            if (dtoPlans == null)
                throw new ArgumentNullException(nameof(dtoPlans));

            List<Models.Plan> listOfPlans = new List<Models.Plan>(0);

            for (int i = 0; i < dtoPlans.Count(); i++)
                listOfPlans.Add(FromDTOModelPlanToModelPlan(dtoPlans.ElementAt(i)));

            return listOfPlans;
        }

        public List<DTOModels.Plan> RangeOfModelsPlanToRangeOfDTOPlans(IEnumerable<Models.Plan> modelPlans)
        {
            if (modelPlans == null)
                throw new ArgumentNullException(nameof(modelPlans));

            List<DTOModels.Plan> plans = new List<DTOModels.Plan>(0);
            
            for (int i = 0; i < modelPlans.Count(); i++)
                plans.Add(FromModelPlanToDTOModelPlan(modelPlans.ElementAt(i)));

            return plans;
        }
    }
}
