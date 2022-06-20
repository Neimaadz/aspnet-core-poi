using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvaluationApi.Models;

namespace EvaluationApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<PointOfInterestItem> PointOfInterestsItems { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Trip> Trips { get; set; }
    }
}
