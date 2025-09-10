using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MunicipalityServices.Models;

namespace MunicipalityServices.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Issue> Issues { get; set; }
        //public DbSet<UserPoints> UserPoints { get; set; }
    }
}
