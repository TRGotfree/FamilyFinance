using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyFinace.Constants;
using FamilyFinace.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFinace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostsController : ControllerBase
    {
        private readonly ICustomLogger logger;
        public CostsController(ICustomLogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Get(DateTime date)
        {
            try
            {

            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode(500, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }
    }
}