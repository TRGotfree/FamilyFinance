﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyFinace.Interfaces
{
    public interface IModelTransformer
    {
        DTOModels.Cost FromModelsCostToDTOModelCost(Models.Cost modelCost);
        Models.Cost FromDTOModelCostToModelCost(DTOModels.Cost dtoModelCost);
        List<DTOModels.Cost> RangeOfModelCostsToRangeOfDTOModelCosts(IEnumerable<Models.Cost> modelCosts);
        List<Models.Cost> RangeOfDTOModelCostsToRangeOfModelCosts(IEnumerable<DTOModels.Cost> dtoModelcosts);
    }
}