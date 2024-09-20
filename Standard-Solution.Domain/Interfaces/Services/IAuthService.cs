using Standard_Solution.Domain.DTOs.Request;
using Standard_Solution.Domain.DTOs.Response;

namespace Standard_Solution.Domain.Interfaces.Services;
public interface IAuthService
{
    Task SignUpUser(UserSignUpRequest newUserRequest, string origin);
    Task<UserLoginResponse> Login(UserLoginRequest userLoginRequest);
    Task SendForgotPasswordEmail(string email, string origin);
    Task ChangePassword(ChangePasswordRequest changePasswordRequest);
    Task VerifyUserEmail(string email, string token);
    Task<UserExistsEmailResponse> SendVerifyEmail(string email, string origin);
    Task EditUser(EditUserRequest alteraUsuarioRequest);
    Task<UserGetResponse> GetUserById(string id);
}
