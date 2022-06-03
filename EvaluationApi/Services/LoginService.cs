using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using EvaluationApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EvaluationApi.Services
{
    public class LoginService : ILoginService
    {
        private IConfiguration _config;

        public LoginService(IConfiguration config)
        {
            _config = config;
        }

        public string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public UserModel Authenticate(UserLogin userLogin)
        {
            var currentUser = UserConstant.Users
                .FirstOrDefault(o =>
                        o.Username.ToLower() == userLogin.Username.ToLower()
                        && o.Password == userLogin.Password
                        );

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
    }
}
