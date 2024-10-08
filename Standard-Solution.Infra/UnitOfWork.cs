﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Standard_Solution.Domain.Interfaces;
using Standard_Solution.Domain.Interfaces.Repositories;
using Standard_Solution.Domain.Models;
using Standard_Solution.Infra.Contexts.SQL;
using Standard_Solution.Infra.Repositories;

namespace Standard_Solution.Infra;
public class UnitOfWork : IUnitOfWork
{
    private readonly Standard_SolutionDbContext Standard_SolutionDbContext;
    public IUserRepository Users { get; }

    public UnitOfWork(Standard_SolutionDbContext dbContext, UserManager<User> userManager)
    {
        Standard_SolutionDbContext = dbContext;
        Standard_SolutionDbContext.Database.SetCommandTimeout(9000);
        Users = new UsersRepository(userManager);
    }

    public int Complete() => Standard_SolutionDbContext.SaveChanges();

    public Task CompleteAsync() => Standard_SolutionDbContext.SaveChangesAsync();

    public void Dispose()
    {
        Standard_SolutionDbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
