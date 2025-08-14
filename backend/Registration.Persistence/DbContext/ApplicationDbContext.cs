using Microsoft.EntityFrameworkCore;
using Registration.Domain.Entities;

namespace Registration.Persistence.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        base.OnModelCreating(modelBuilder);

        // Ensure Role_Id is unique
        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Role_Id)
            .IsUnique();
        }
    }

    
}
