using System;
using EvaluationApi.Models;

namespace EvaluationApi.Services
{
    public interface ILoginService
    {
        public string Generate(UserModel user);

        public UserModel Authenticate(UserLogin userLogin);
    }
}
