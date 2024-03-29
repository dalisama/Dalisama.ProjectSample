﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Dalisama.ProjectSample.Gateway.Controllers
{
    [Route("[action]")]
    public class HealthController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        [ActionName("health")]
        public ActionResult<string> Get([FromServices]IWebHostEnvironment hostingEnvironement)
        {
            return Ok($"I'm alive in the {hostingEnvironement.EnvironmentName}.");
        }
        [HttpGet]
        [ActionName("badhealth")]
        public ActionResult<string> GetException()
        {
            throw new Exception();
        }



    }
}
