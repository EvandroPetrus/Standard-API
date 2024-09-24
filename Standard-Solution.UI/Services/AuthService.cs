using System.Net.Http.Json;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResult> Login(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5013/api/Auth/Login/signin", loginRequest);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            return result;
        }
        return new LoginResult { Success = false };
    }
    public async Task<ForgotPasswordResult> SendForgotPasswordEmail(ForgotPasswordRequest forgotPasswordRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5013/api/Auth/SendForgotPasswordEmail/forgot-password", forgotPasswordRequest);
        if (response.IsSuccessStatusCode)
        {
            return new ForgotPasswordResult { Success = true };
        }
        return new ForgotPasswordResult { Success = false };
    }

    public async Task<SignUpResult> SignUpUser(SignUpRequest signUpRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5013/api/Auth/Register/SignUp", signUpRequest);
        if (response.IsSuccessStatusCode)
        {
            return new SignUpResult { Success = true, Message = "Email sent to user" };
        }
        return new SignUpResult { Success = false, Message = "Error at signing up user" };
    }
}