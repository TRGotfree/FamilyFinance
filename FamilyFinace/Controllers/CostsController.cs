using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FamilyFinace.Constants;
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

        public CostsController(ICustomLogger logger, IModelTransformer modelTransformer, IRepository repository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.modelTransformer = modelTransformer ?? throw new ArgumentNullException(nameof(modelTransformer));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
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

                return StatusCode((int)HttpStatusCode.Created, addedCost);
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

                return StatusCode((int)HttpStatusCode.Created, updatedCost);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }
    }
}