using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FamilyFinace.Constants;
using FamilyFinace.Interfaces;
using FamilyFinace.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFinace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly ICustomLogger logger;
        private readonly IRepository repository;

        public StoresController(ICustomLogger customLogger, IRepository repository)
        {
            this.logger = customLogger ?? throw new ArgumentNullException(nameof(logger));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var stores = await repository.GetStores();
                if (stores == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.DATA_NOT_FOUND);

                return Ok(stores);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Store store)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var newStore = await repository.AddStore(store);
                if (newStore == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.DATA_NOT_FOUND);

                return StatusCode((int)HttpStatusCode.Created, newStore);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Store store)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var updatedPayType = await repository.UpdateStore(store);
                if (updatedPayType == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.DATA_NOT_FOUND);

                return StatusCode((int)HttpStatusCode.Created, updatedPayType);
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
                await repository.DeleteStore(id);

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