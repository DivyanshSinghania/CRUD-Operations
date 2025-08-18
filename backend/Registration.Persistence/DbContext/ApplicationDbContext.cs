using Microsoft.EntityFrameworkCore;
using Registration.Domain.Entities;

namespace Registration.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Technology> Technologies { get; set; }
    }
}
