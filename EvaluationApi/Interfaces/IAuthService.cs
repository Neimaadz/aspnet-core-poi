using System;
using System.Threading.Tasks;
using EvaluationApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EvaluationApi.Services
{
    public interface IAuthService
    {
        public AuthPayload Authenticate(UserLogin userLogin);
        public AuthPayload Register(User newUser);
    }
}
