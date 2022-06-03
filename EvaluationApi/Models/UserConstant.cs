using System;
using System.Collections.Generic;

namespace EvaluationApi.Models
{
    public class UserConstant
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() { Username = "admin", Password = "admin", Role = "Administrator" },
            new UserModel() { Username = "user", Password = "user", Role = "User" },
        };
    }
}
