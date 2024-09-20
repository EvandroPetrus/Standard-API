using Standard_Solution.Domain.DTOs.Request;
using Standard_Solution.Domain.DTOs.Response;
using Standard_Solution.Domain.Interfaces.Services;
using AutoMapper;
using Standard_Solution.Domain.Interfaces;

namespace Standard_Solution.Service.Services;

public class AuthService : IAuthService
{
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public AuthService(IEmailService emailService,
                       IUnitOfWork unitOfWork,
                       IMapper mapper,
                       ITokenService tokenService)
    {
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public Task ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        throw new NotImplementedException();
    }

    public Task EditUser(EditUserRequest alteraUsuarioRequest)
    {
        throw new NotImplementedException();
    }

    public Task<UserGetResponse> GetUserById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<UserLoginResponse> Login(UserLoginRequest userLoginRequest)
    {
        throw new NotImplementedException();
    }

    public Task SendForgotPasswordEmail(string email, string origin)
    {
        throw new NotImplementedException();
    }

    public Task<UserExistsEmailResponse> SendVerifyEmail(string email, string origin)
    {
        throw new NotImplementedException();
    }

    public Task SignUpUser(UserSignUpRequest newUserRequest, string origin)
    {
        throw new NotImplementedException();
    }

    public Task VerifyUserEmail(string email, string token)
    {
        throw new NotImplementedException();
    }
}
