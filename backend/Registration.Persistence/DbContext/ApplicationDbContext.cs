using Microsoft.EntityFrameworkCore;
using Registration.Domain.Entities;

namespace Registration.Persistence.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<BudgetCategory> BudgetCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<BudgetCategory>()
        .Property(b => b.Id)
        .ValueGeneratedNever();
}

    }
}