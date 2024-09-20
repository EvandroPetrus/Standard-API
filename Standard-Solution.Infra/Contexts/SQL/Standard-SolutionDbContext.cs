using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Standard_Solution.Domain.Models;

namespace Standard_Solution.Infra.Contexts.SQL
{
    public class Standard_SolutionDbContext : IdentityDbContext<User>
    {
        public Standard_SolutionDbContext(DbContextOptions<Standard_SolutionDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Standard_SolutionDbContext).Assembly);
        }
    }
}
