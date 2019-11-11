using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dalisama.ProjectSimple.Gateway.Controllers
{
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        [ActionName("Health")]
        public ActionResult<string> Get()
        {
            return Ok("im ok");
        }
        [HttpGet]
        [ActionName("BadHealth")]
        public ActionResult<string> GetException()
        {
            throw new Exception();
        }



    }
}
