using System;
using Microsoft.AspNetCore.Mvc;

namespace Dalisama.ProjectSample.Gateway.Controllers
{
    [Route("[action]")]
    public class HealthController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        [ActionName("health")]
        public ActionResult<string> Get()
        {
            return Ok("im ok");
        }
        [HttpGet]
        [ActionName("badhealth")]
        public ActionResult<string> GetException()
        {
            throw new Exception();
        }



    }
}
