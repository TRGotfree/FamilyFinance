using System;
using System.Net;
using System.Threading.Tasks;
using FamilyFinace.Constants;
using FamilyFinace.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFinace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly ICustomLogger logger;
        private readonly IRepository repository;
        public StatisticsController(ICustomLogger logger, IRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet("{month:int}/{year:int}")]
        public async Task<IActionResult> Get(int month, int year)
        {
            try
            {
                if (month <= 0)
                    return BadRequest();

                if (year <= 0)
                    return BadRequest();

                var statistic = await repository.GetStatistic(month, year);
                return Ok(statistic);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }
   
        [HttpGet("{month:int}/{year:int}/paytypes")]
        public async Task<IActionResult> GetConstsByPayTypes(int month, int year)
        {
            try
            {
                if (month <= 0)
                    return BadRequest();

                if (year <= 0)
                    return BadRequest();

                //TODO:
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }
    }
}
