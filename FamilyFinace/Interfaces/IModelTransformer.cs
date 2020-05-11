using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Interfaces
{
    public interface IModelTransformer
    {
        public DTOModels.Cost FromModelsCostToDTOModelCost(Models.Cost modelCost);
        public Models.Cost FromDTOModelCostToModelCost(DTOModels.Cost dtoModelCost);
        public List<DTOModels.Cost> RangeOfModelCostsToRangeOfDTOModelCosts(IEnumerable<Models.Cost> modelCosts);
        public List<Models.Cost> RangeOfDTOModelCostsToRangeOfModelCosts(IEnumerable<DTOModels.Cost> dtoModelcosts);

        public DTOModels.Plan FromModelPlanToDTOModelPlan(Models.Plan modelPlan);
        public Models.Plan FromDTOModelPlanToModelPlan(DTOModels.Plan dtoPlan);
        public List<DTOModels.Plan> RangeOfModelsPlanToRangeOfDTOPlans(IEnumerable<Models.Plan> modelPlans);
        public List<Models.Plan> RangeOfDTOModelsPlanToRangeOfModelPlans(IEnumerable<DTOModels.Plan> dtoPlans);

    }
}
