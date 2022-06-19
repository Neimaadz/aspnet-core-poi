using Microsoft.AspNetCore.Mvc;
using EvaluationApi.Models;
using EvaluationApi.Services;

namespace EvaluationApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthService _service;

        public AuthController(AuthService service)
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
