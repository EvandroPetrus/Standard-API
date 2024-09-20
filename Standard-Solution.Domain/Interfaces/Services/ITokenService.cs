using Standard_Solution.Domain.DTOs.Response;

namespace Standard_Solution.Domain.Interfaces.Services;

public interface ITokenService
{
    Task<UserLoginResponse> GenerateCredentials(string email);
}
