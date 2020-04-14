using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyFinace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        public IndexController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public IActionResult Index()
        {
            return new PhysicalFileResult(Path.Combine(env.WebRootPath, "index.html"), "text/html");
        }
    }
}