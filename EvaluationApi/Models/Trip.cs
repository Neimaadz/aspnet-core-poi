using System.Collections.Generic;

namespace EvaluationApi.Models
{
    public class Trip
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public List<PointOfInterestItem> Pois { get; set; }
    }
}
