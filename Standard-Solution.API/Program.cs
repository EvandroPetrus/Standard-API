using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Standard_Solution.API.Extensions;
using Standard_Solution.Domain.Models;
using Standard_Solution.Infra.Contexts.SQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomCors();

builder.Services.AddControllers();
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

builder.Services.AddDbContext<Standard_SolutionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Standard_SolutionConnectionString"),
    sqlOptions => sqlOptions.MigrationsAssembly("Standard-Solution.Infra")));

var app = builder.Build();

// UNCOMMENT for LOCAL use **ONLY**!!!
// (auto-applies migrations, so it can mess up with version control)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<Standard_SolutionDbContext>();

    context.Database.Migrate();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
