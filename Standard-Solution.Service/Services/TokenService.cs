using Standard_Solution.Domain.DTOs.Response;
using Standard_Solution.Domain.Interfaces.Services;

namespace Standard_Solution.Service.Services;

public class TokenService : ITokenService
{
    public Task<UserLoginResponse> GenerateCredentials(string email)
    {
        throw new NotImplementedException();
    }
}
