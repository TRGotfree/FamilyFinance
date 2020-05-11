using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FamilyFinace.Constants;
using FamilyFinace.DTOModels;
using FamilyFinace.Interfaces;
using FamilyFinace.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFinace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly ICustomLogger logger;
        private readonly IModelTransformer modelTransformer;
        private readonly IRepository repository;
        private readonly IModelMetaDataProvider modelMetaDataProvider;
        public PlansController(ICustomLogger logger, IModelTransformer modelTransformer, IRepository repository, IModelMetaDataProvider modelMetaDataProvider)
        {
            this.logger = logger;
            this.modelTransformer = modelTransformer;
            this.repository = repository;
            this.modelMetaDataProvider = modelMetaDataProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery][Range(1, 12)]int month, [FromQuery][Range(2020, 2200)]int year)
        {
            try
            {
                var plans = await repository.GetPlansWithMaxAmountOfCosts(month, year);

                return Ok(modelTransformer.RangeOfModelsPlanToRangeOfDTOPlans(plans));
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, Constants.ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }
    
    
        [HttpGet]
        [Route("meta")]
        public IActionResult Get()
        {
            try
            {
                var propsAndDisplayNames = modelMetaDataProvider.GetMeta<Plan>();
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
    }
}