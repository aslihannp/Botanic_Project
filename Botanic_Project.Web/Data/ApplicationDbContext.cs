using Botanic_Project.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Botanic_Project.Web.Data
{
    public class ApplicationDbContext : DbContext //DbContext Eklenen Nuget paketlerden geldi
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Plant> Plant { get; set; }
        public DbSet<User> User { get; set; }

    }
}
