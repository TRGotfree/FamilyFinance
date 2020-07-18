using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyFinace.Interfaces;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
