using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Serilog;
using Standard_Solution.API.Extensions;
using Standard_Solution.API.Filter;
using Standard_Solution.Domain.Interfaces.Services;
using Standard_Solution.Domain.Models;
using Standard_Solution.Infra.Contexts.SQL;
using Standard_Solution.Service.Services.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilogConfiguration(builder.Configuration);
builder.Host.UseSerilog();

builder.Services.AddCustomCors();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddAutoMapperProfiles();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<Standard_SolutionDbContext>().AddDefaultTokenProviders();

var connectionString = builder.Configuration.GetConnectionString("Standard_SolutionConnectionString");
if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("Connection string 'Standard_SolutionConnectionString' is not configured.");


builder.Services.AddDbContext<Standard_SolutionDbContext>(options =>
    options.UseSqlServer(connectionString,
    sqlOptions => sqlOptions.MigrationsAssembly("Standard-Solution.Infra")));

var app = builder.Build();
try
{
    Log.Information("Starting web API");

    // UNCOMMENT for LOCAL use **ONLY**!!!
    // (auto-applies migrations, so it can mess up with version control)
    using (var scope = app.Services.CreateScope())
    {
        var rabbitMqConnectionService = scope.ServiceProvider.GetRequiredService<IRabbitMqConnectionService>();
        if (!rabbitMqConnectionService.TryConnect())
            throw new Exception("Failed to connect to RabbitMQ.");

        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<Standard_SolutionDbContext>();
        var migrator = context.GetService<IMigrator>();
        var pendingMigrations = context.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            foreach (var migration in pendingMigrations)
            {
                Log.Information($"Applying migration: {migration}");
                migrator.Migrate(migration);
            }
        }
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowSpecificOrigin");
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}