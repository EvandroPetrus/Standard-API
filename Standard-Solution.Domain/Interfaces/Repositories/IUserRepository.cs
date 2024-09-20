using Microsoft.AspNetCore.Identity;
using Standard_Solution.Domain.Models;

namespace Standard_Solution.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserById(string id);
        Task<User> GetUserByEmail(string email);
        Task<IdentityResult> ChangePassword(User user, string token, string password);
        Task<string> GenerateTokenResetPassword(User user);
        Task<string> GenerateTokenVerifyEmail(User user);
        Task<IdentityResult> InsertUser(User user, string password);
        Task<bool> CheckUserPassword(User user, string password);
        Task<IdentityResult> ConfirmUserEmail(User user, string token);
        Task<IdentityResult> EditUser(User user);
    }
}
