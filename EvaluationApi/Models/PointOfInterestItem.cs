using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationApi.Models
{
    public class PointOfInterestItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Comment { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
