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

        public PointOfInterestItem(long Id, string Name, string ImagePath, string Comment, double Lat, double Lng)
        {
            this.Id = Id;
            this.Name = Name;
            this.ImagePath = ImagePath;
            this.Comment = Comment;
            this.Lat = Lat;
            this.Lng = Lng;
        }
        public PointOfInterestItem(string Name, string ImagePath, string Comment, double Lat, double Lng)
        {
            this.Name = Name;
            this.ImagePath = ImagePath;
            this.Comment = Comment;
            this.Lat = Lat;
            this.Lng = Lng;
        }

    }

}
