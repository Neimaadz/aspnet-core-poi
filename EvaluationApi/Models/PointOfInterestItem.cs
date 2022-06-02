using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationApi.Models
{
    public class PointOfInterestItem
    {
        public long Id { get; set; }
        public string name { get; set; }
        public string imagePath { get; set; }
        public string comment { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }
}
