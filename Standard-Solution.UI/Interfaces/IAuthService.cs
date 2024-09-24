public interface IAuthService
{
    Task<LoginResult> Login(LoginRequest loginrequest);
    Task<ForgotPasswordResult> SendForgotPasswordEmail(ForgotPasswordRequest forgotPasswordRequest);
    Task<SignUpResult> SignUpUser(SignUpRequest signUpRequest);
}