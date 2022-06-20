using System.Collections.Generic;

namespace EvaluationApi.Models
{
    public class TripDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public List<long> PoisId { get; set; }
    }
}
