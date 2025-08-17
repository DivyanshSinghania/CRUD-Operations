using Microsoft.EntityFrameworkCore;
using Registration.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Registration.Persistence.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // No DateTime processing here
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
