using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaluationApi.Models;
using EvaluationApi.Services;
using System.Net;
using System.Net.Http;

namespace EvaluationApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _service;

        public AuthController(IAuthService service)
        {;
            _service = service;
        }

        /// <summary>
        /// We verify the connexion here and return if authorized or not
        /// </summary>
        /// <returns>HTTP code</returns>
        /// <remarks>Basic post auth for the login</remarks>

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            try {
                //Vérification de la connexion
                AuthPayload authPayload = _service.Authenticate(userLogin);

                if (authPayload != null)
                {
                    return Ok(new { authPayload.User, authPayload.Token });
                }
            }
            catch { }

            return Unauthorized("Username or password invalid");
        }


        /// <summary>
        /// Here we register our username and password
        /// </summary>
        /// <returns>HTTP code</returns>
        /// <remarks>The api verify if the user are already use or not</remarks>
        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            AuthPayload authPayload = _service.Register(newUser);

            if (authPayload != null)
            {
                return Ok(new { authPayload.User, authPayload.Token });
            }
            return Conflict(string.Format("{0} already use !", newUser.Username));
        }
    }
}
