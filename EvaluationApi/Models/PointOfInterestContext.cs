using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationApi.Models
{
    public class PointOfInterestContext : DbContext
    {
        public PointOfInterestContext(DbContextOptions<PointOfInterestContext> options) : base(options)
        {

        }

        public DbSet<PointOfInterestItem> PointOfInterestsItems { get; set; }
    }
}
