using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FamilyFinace.Constants;
using FamilyFinace.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFinace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomesController : ControllerBase
    {
        private readonly ICustomLogger logger;
        private readonly IRepository repository;
        private readonly IModelTransformer modelTransformer;
        private readonly IModelMetaDataProvider modelMetaDataProvider;

        public IncomesController(ICustomLogger logger, IRepository repository,
            IModelTransformer modelTransformer, IModelMetaDataProvider modelMetaDataProvider)
        {
            this.logger = logger;
            this.repository = repository;
            this.modelTransformer = modelTransformer;
            this.modelMetaDataProvider = modelMetaDataProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DateTime beginDate, DateTime endDate)
        {
            try
            {
                var incomes = await repository.GetIncomes(beginDate, endDate);
                return Ok(incomes);
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
                var propsAndDisplayNames = modelMetaDataProvider.GetMeta<DTOModels.Income>();
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
        public async Task<IActionResult> Post(DTOModels.Income newIncome)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var income = modelTransformer.FromDTOModelIncomeToModelIncome(newIncome);
                var savedIncome = await repository.AddIncome(income);
                
                if (savedIncome == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);

                return StatusCode((int)HttpStatusCode.Created, modelTransformer.FromModelIncomeToDTOModelIncome(savedIncome));
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(DTOModels.Income editedIncome)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var income = modelTransformer.FromDTOModelIncomeToModelIncome(editedIncome);
                var savedIncome = await repository.UpdateIncome(income);

                if (savedIncome != null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);

                return StatusCode((int)HttpStatusCode.Created, modelTransformer.FromModelIncomeToDTOModelIncome(savedIncome));
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int incomeId)
        {
            try
            {
                if (incomeId <= 0)
                    return BadRequest();

                await repository.DeleteIncome(incomeId);

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
