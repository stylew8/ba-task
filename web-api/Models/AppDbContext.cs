using Microsoft.EntityFrameworkCore;

namespace web_api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ElectricityData> ElectricityData { get; set; }
    }
}
