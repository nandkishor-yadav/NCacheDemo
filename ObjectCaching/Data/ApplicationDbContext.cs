using Microsoft.EntityFrameworkCore;
using ObjectCaching.Models;

namespace ObjectCaching.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<City> Cities { get; set; }
    }
}
