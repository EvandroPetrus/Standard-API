using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Standard_Solution.Infra.Contexts.SQL.Migrations
{
    public class Standard_SolutionDbContextFactory : IDesignTimeDbContextFactory<Standard_SolutionDbContext>
    {
        public Standard_SolutionDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Standard_SolutionDbContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Standard-Solution.API"))
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Standard_SolutionConnectionString");

            optionsBuilder.UseSqlServer(connectionString);

            return new Standard_SolutionDbContext(optionsBuilder.Options);
        }
    }
}