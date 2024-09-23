using AutoMapper;
using Standard_Solution.Domain.DTOs.Request;
using Standard_Solution.Domain.DTOs.Response;
using Standard_Solution.Domain.Interfaces;
using Standard_Solution.Domain.Interfaces.Services;
using Standard_Solution.Domain.Models;

namespace Standard_Solution.Service.Services;

public class AuthService : IAuthService
{
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRabbitMqService _rabbitMqService;
    public AuthService(IEmailService emailService,
                       IUnitOfWork unitOfWork,
                       IMapper mapper,
                       ITokenService tokenService,
                       IRabbitMqService rabbitMqService)
    {
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tokenService = tokenService;
        _rabbitMqService = rabbitMqService;
    }
    public async Task SignUpUser(UserSignUpRequest newUserRequest, string origin)
    {
        if (await _unitOfWork.Users.GetUserByEmail(newUserRequest.Email) is not null)
            throw new InvalidOperationException("Email '" + newUserRequest.Email + "' is already registered.");

        var user = _mapper.Map<User>(newUserRequest);
        user.UserName = newUserRequest.Email;

        var result = await _unitOfWork.Users.InsertUser(user, newUserRequest.Password);

        if (result.Succeeded)
        {
            string token = await _unitOfWork.Users.GenerateTokenVerifyEmail(user);

            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "E-mail confirmation", "VerifyAccount.html");

            var emailConfig = new EmailConfigurations(user.Email, user.UserName, templatePath, "Verify your email");
            _rabbitMqService.PublishMessage(emailConfig);
        }
        else
            throw new InvalidOperationException("User registration failed! Please check user details and try again.");
    }

    public async Task<UserLoginResponse> Login(UserLoginRequest userLoginRequest)
    {
        var user = await _unitOfWork.Users.GetUserByEmail(userLoginRequest.Email);

        var validUser = await _unitOfWork.Users.CheckUserPassword(user, userLoginRequest.Password);

        if (user is null || !validUser || !user.EmailConfirmed)
            return new UserLoginResponse(false, null, null);

        if (validUser)
            return await _tokenService.GenerateCredentials(user.Email);
        else
            throw new InvalidOperationException("Invalid user email or password.");

    }
    public async Task<UserExistsEmailResponse> SendVerifyEmail(string email, string origin)
    {
        User user = await _unitOfWork.Users.GetUserByEmail(email);

        if (user is null)
            return new UserExistsEmailResponse { UserExists = false, Email = email };
        else
        {
            string token = await _unitOfWork.Users.GenerateTokenVerifyEmail(user);

            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "E-mail confirmation", "VerifyAccount.html");

            var emailConfig = new EmailConfigurations(user.Email, user.UserName, templatePath, "Verify your email");
            _rabbitMqService.PublishMessage(emailConfig);

            var response = _mapper.Map<User, UserExistsEmailResponse>(user);
            response.UserExists = true;
            return response;
        }
    }

    public async Task VerifyUserEmail(string email, string token)
    {
        var user = await _unitOfWork.Users.GetUserByEmail(email) ?? throw new InvalidOperationException("User not found.");

        var result = await _unitOfWork.Users.ConfirmUserEmail(user, token);

        if (!result.Succeeded)
            throw new InvalidOperationException("Email verification failed! Please check your email and try again.");
        else
            user.EmailConfirmed = true;

    }
    public async Task ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        User user = await _unitOfWork.Users.GetUserByEmail(changePasswordRequest.Email) ?? throw new InvalidOperationException("User not found.");

        var result = await _unitOfWork.Users.ChangePassword(user, changePasswordRequest.Token.Replace(" ", "+"), changePasswordRequest.NewPassword);

        if (!result.Succeeded)
            throw new InvalidOperationException("Password change failed! Please check your email and try again.");
    }
    public async Task EditUser(EditUserRequest editUserRequest)
    {
        User user = await _unitOfWork.Users.GetUserById(editUserRequest.Id) ?? throw new InvalidOperationException("User not found.");

        _mapper.Map(editUserRequest, user);

        var result = await _unitOfWork.Users.EditUser(user);

        if (!result.Succeeded)
            throw new InvalidOperationException("User update failed! Please check your data and try again.");
    }
    public async Task<UserGetResponse> GetUserById(Guid id)
    {
        User user = await _unitOfWork.Users.GetUserById(id) ?? throw new InvalidOperationException("User not found.");

        UserGetResponse response = _mapper.Map<UserGetResponse>(user);
        return response;
    }
    public async Task SendForgotPasswordEmail(string email, string origin)
    {
        var user = await _unitOfWork.Users.GetUserByEmail(email) ?? throw new InvalidOperationException("User not found for this email.");

        string token = await _unitOfWork.Users.GenerateTokenResetPassword(user);

        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "Password reset", "ForgotPassword.html");

        var emailConfig = new EmailConfigurations(user.Email, user.UserName, templatePath, "Reset your password");
        _rabbitMqService.PublishMessage(emailConfig);
    }
}
