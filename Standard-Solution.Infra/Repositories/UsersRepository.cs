using Microsoft.AspNetCore.Identity;
using Standard_Solution.Domain.Interfaces.Repositories;
using Standard_Solution.Domain.Models;

namespace Standard_Solution.Infra.Repositories
{
    public class UsersRepository(UserManager<User> userManager) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<IdentityResult> ChangePassword(User user, string token, string password) =>
            await _userManager.ChangePasswordAsync(user, token, password);

        public async Task<bool> CheckUserPassword(User user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);

        public async Task<IdentityResult> ConfirmUserEmail(User user, string token) =>
            await _userManager.ConfirmEmailAsync(user, token);

        public async Task<IdentityResult> EditUser(User user) =>
            await _userManager.UpdateAsync(user);

        public async Task<string> GenerateTokenResetPassword(User user) =>
            await _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<string> GenerateTokenVerifyEmail(User user) =>
            await _userManager.GenerateEmailConfirmationTokenAsync(user);

        public async Task<User> GetUserByEmail(string email) =>
            await _userManager.FindByEmailAsync(email);

        public async Task<User> GetUserById(Guid id) =>
            await _userManager.FindByIdAsync(id.ToString());

        public async Task<IdentityResult> InsertUser(User user, string password) =>
            await _userManager.CreateAsync(user, password);
    }
}
