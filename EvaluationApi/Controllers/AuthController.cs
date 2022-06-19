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
