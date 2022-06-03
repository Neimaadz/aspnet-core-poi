using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvaluationApi.Models;
using EvaluationApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EvaluationApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService _service;

        public LoginController(ILoginService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            //Vérification de la connexion
            var user = _service.Authenticate(userLogin);

            if (user != null)
            {
                var token = _service.Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }
    }

}