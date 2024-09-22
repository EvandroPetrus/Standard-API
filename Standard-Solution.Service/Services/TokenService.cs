using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Standard_Solution.Domain.DTOs.Response;
using Standard_Solution.Domain.Interfaces.Services;
using Standard_Solution.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Standard_Solution.Service.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public TokenService(IConfiguration configuration, UserManager<User> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<UserLoginResponse> GenerateCredentials(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var accessTokenClaims = await GetClaimsAsync(user, true);

        TimeZoneInfo standard_time = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        DateTime thisDate = TimeZoneInfo.ConvertTime(DateTime.Now, standard_time);

        DateTime dateExpires = thisDate.AddHours(double.Parse(_configuration["JwtOptions:AccessTokenExpiration"]));

        var token = GenerateToken(accessTokenClaims, dateExpires);

        return new UserLoginResponse(true, token, dateExpires);
    }

    #region Helpers
    private string GenerateToken(IEnumerable<Claim> claims, DateTime expireDate)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:SecurityKey"]));

        var jwt = new JwtSecurityToken(
            issuer: _configuration["JwtOptions:Issuer"],
            audience: _configuration["JwtOptions:Audience"],
            claims: claims,
            expires: expireDate,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private async Task<IList<Claim>> GetClaimsAsync(User user, bool addUserClaims)
    {
        var claims = AddClaimTypes(user);

        if (addUserClaims)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            claims.AddRange(userClaims);
        }

        return claims;
    }

    private static List<Claim> AddClaimTypes(User user)
    {
        return
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        ];
    }
    #endregion
}
