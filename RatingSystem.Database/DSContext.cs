using Microsoft.AspNet.Identity.EntityFramework;
using RatingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingSystem.Database
{
    public class DSContext :IdentityDbContext<User>,IDisposable
    {
        public DSContext() : base("RSConnectionStrings")
        {

        }

        public static DSContext Create()
        {
            return new DSContext();
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Employee> Employees { get; set; }


    }
}
