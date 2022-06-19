using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using EvaluationApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace EvaluationApi.Services
{
    public class AuthService
    {
        private IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        public AuthPayload Authenticate(UserLogin userLogin)
        {
            try
            {
                // Verify username exist in database
                User currentUser = _context.User.FirstOrDefault(user => user.Username.ToLower() == userLogin.Username.ToLower());

                if (currentUser != null && BC.Verify(userLogin.Password, currentUser.Password))
                {
                    string token = Generate(currentUser);

                    return new AuthPayload { User = currentUser, Token = token };
                }

                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AuthPayload Register(User newUser)
        {
            // Verify username already exist or not
            IEnumerable<User> users = _context.User.ToList();
            User user = users.FirstOrDefault(user => user.Username.ToLower() == newUser.Username.ToLower());

            if (user == null)
            {
                // Hash password
                newUser.Password = BC.HashPassword(newUser.Password);
                newUser.Role = "user";

                _context.User.Add(newUser);
                _context.SaveChanges();

                UserLogin userLogin = new UserLogin { Username = newUser.Username, Password = newUser.Password };
                AuthPayload authPayload = Authenticate(userLogin);

                return new AuthPayload { User = authPayload.User, Token = authPayload.Token };
            }
            return null;
        }




        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
