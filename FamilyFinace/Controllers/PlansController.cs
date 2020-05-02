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
    public class PlansController : ControllerBase
    {
        public PlansController(ICustomLogger logger, IModelTransformer modelTransformer, IRepository repository)
        {

        }
    }
}