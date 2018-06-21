using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        [Route("details")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(
                new {
                    Name = User.Identity.Name,
                    Claims = User.Claims.Select(x => new { x.Type, x.Value }).ToList()
                });
        }
    }
}
