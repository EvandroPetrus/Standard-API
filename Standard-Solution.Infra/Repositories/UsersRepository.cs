using Microsoft.AspNetCore.Identity;
using Standard_Solution.Domain.Interfaces.Repositories;
using Standard_Solution.Domain.Models;
        
namespace Standard_Solution.Infra.Repositories;

public class UsersRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UsersRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> ChangePassword(User user, string token, string password)
        => await _userManager.ChangePasswordAsync(user, token, password);

    public async Task<bool> CheckUserPassword(User user, string password)
        => await _userManager.CheckPasswordAsync(user, password);

    public Task<IdentityResult> ConfirmUserEmail(User user, string token)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> EditUser(User user)
    {
        throw new NotImplementedException();
    }

    public Task<string> GenerateTokenResetPassword(User user)
    {
        throw new NotImplementedException();
    }

    public Task<string> GenerateTokenVerifyEmail(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> InsertUser(User user, string password)
    {
        throw new NotImplementedException();
    }
}
