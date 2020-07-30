using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using FamilyFinace.Constants;
using FamilyFinace.DTOModels;
using FamilyFinace.Interfaces;
using FamilyFinace.Models;
using FamilyFinace.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFinace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostsController : ControllerBase
    {
        private readonly ICustomLogger logger;
        private readonly IRepository repository;
        private readonly IModelTransformer modelTransformer;
        private readonly IModelMetaDataProvider modelMetaDataProvider;

        public CostsController(ICustomLogger logger, IModelTransformer modelTransformer, IRepository repository, IModelMetaDataProvider modelMetaDataProvider)
        {
            this.logger = logger;
            this.modelTransformer = modelTransformer;
            this.repository = repository;
            this.modelMetaDataProvider = modelMetaDataProvider;
        }

        [HttpGet("{date}")]
        public async Task<IActionResult> Get(DateTime date)
        {
            try
            {
                if (date == null)
                    return BadRequest();

                var costs = await repository.GetCosts(date);
                if (costs == null)
                    return StatusCode((int)HttpStatusCode.NotFound);

                var dtoCosts = modelTransformer.RangeOfModelCostsToRangeOfDTOModelCosts(costs);

                return Ok(dtoCosts);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }

        [HttpGet()]
        [Route("meta")]
        public IActionResult Get()
        {
            try
            {
                var propsAndDisplayNames = modelMetaDataProvider.GetMeta<DTOModels.Cost>();
                if (propsAndDisplayNames == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);

                return Ok(propsAndDisplayNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(DTOModels.Cost cost)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var modelCost = modelTransformer.FromDTOModelCostToModelCost(cost);

                var addedCost = await repository.AddCost(modelCost);
                if (addedCost == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);

                return StatusCode((int)HttpStatusCode.Created, modelTransformer.FromModelsCostToDTOModelCost(addedCost));
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }
    
        public async Task<IActionResult> Put(DTOModels.Cost cost)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var modelCost = modelTransformer.FromDTOModelCostToModelCost(cost);

                var updatedCost = await repository.UpdateCost(modelCost);

                if (updatedCost == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);

                return StatusCode((int)HttpStatusCode.Created, modelTransformer.FromModelsCostToDTOModelCost(updatedCost));
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }
    
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await repository.DeleteCost(id);
                return StatusCode((int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }
    }
}