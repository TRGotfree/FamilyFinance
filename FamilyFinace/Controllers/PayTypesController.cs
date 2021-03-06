﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FamilyFinace.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FamilyFinace.Constants;
using FamilyFinace.Models;

namespace FamilyFinace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayTypesController : ControllerBase
    {
        private readonly ICustomLogger logger;
        private readonly IRepository repository;

        public PayTypesController(ICustomLogger customLogger, IRepository repository)
        {
            this.logger = customLogger ?? throw new ArgumentNullException(nameof(logger));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var payTypes = await repository.GetPayTypes();
                if (payTypes == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.DATA_NOT_FOUND);

                return Ok(payTypes);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(PayType payType)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var newPayType = await repository.AddPayType(payType);
                if (newPayType == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.DATA_NOT_FOUND);

                return StatusCode((int)HttpStatusCode.Created, newPayType);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(PayType payType)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var updatedPayType = await repository.UpdatePayType(payType);
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
                await repository.DeletePayType(id);
                
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