using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EvaluationApi.Models
{
    public interface IPointOfInterestsRepository
    {
        public List<PointOfInterestItem> GetAll();
    }
 
}
