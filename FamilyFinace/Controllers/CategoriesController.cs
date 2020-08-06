using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FamilyFinace.Constants;
using FamilyFinace.DTOModels;
using FamilyFinace.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFinace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICustomLogger logger;
        private readonly IRepository repository;
        private readonly IModelTransformer modelTransformer;
        private readonly IModelMetaDataProvider modelMetaDataProvider;

        public CategoriesController(ICustomLogger logger, IRepository repository,
            IModelTransformer modelTransformer, IModelMetaDataProvider modelMetaDataProvider)
        {
            this.logger = logger;
            this.repository = repository;
            this.modelTransformer = modelTransformer;
            this.modelMetaDataProvider = modelMetaDataProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categories = await repository.GetCostCategories();
                if (categories == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.DATA_NOT_FOUND);

                return Ok(categories);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Category category)
        {
            try
            {
                if (category == null || !ModelState.IsValid)
                    return BadRequest();

                var newCategorie = await repository.AddCostCategory(category);
                if (newCategorie == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);

                return StatusCode((int)HttpStatusCode.Created, newCategorie);
            }
            catch (Exception ex) 
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Category category)
        {
            try
            {
                if (category != null || !ModelState.IsValid)
                    return BadRequest();

                var updatedCategorie = await repository.UpdateCostCategory(category);
                if (updatedCategorie == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);

                return StatusCode((int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ServerMessages.INTERNAL_SERVER_ERROR);
            }
        }
   
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                await repository.DeleteCostCategory(id);
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
