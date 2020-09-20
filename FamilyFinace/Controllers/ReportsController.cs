using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FamilyFinace.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFinace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ICustomLogger logger;
        private readonly IRepository repository;
        public ReportsController(ICustomLogger logger, IRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public async Task<IActionResult> GetCostsByPeriod(DateTime beginDate, DateTime endDate)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            { 
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, Constants.ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }
    }
}
