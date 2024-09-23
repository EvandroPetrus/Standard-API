using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Standard_Solution.Domain.DTOs.Request;
using Standard_Solution.Domain.DTOs.Response;
using Standard_Solution.Domain.Interfaces.Services;

namespace Standard_Solution.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly Serilog.ILogger _logger;

    public AuthController(IAuthService authService, Serilog.ILogger logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// User login endpoint.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<ActionResult<UserLoginResponse>> Login([FromBody] UserLoginRequest userLoginData)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var loginResult = await _authService.Login(userLoginData);
            if (!loginResult.Success)
            {
                _logger.Warning($"There was a failed attempt to log into this email: {userLoginData.Email}, Time: {DateTime.UtcNow}, User Agent: {Request.Headers.UserAgent}", userLoginData);
                return Unauthorized("The e-mail or password doesn't match an existing user");
            }
            return Ok(loginResult);
        }
        catch (Exception ex)
        {
            // Custom log error with relevant details
            _logger.Error(ex,
                "Error during login. Email: {Email}, Time: {Time}, User Agent: {UserAgent}",
                userLoginData.Email, DateTime.UtcNow, Request.Headers.UserAgent);

            return BadRequest(new { Message = "An error occurred during login. Please try again later." });
        }
    }

    /// <summary>
    /// User registration endpoint.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] UserSignUpRequest userSignUpData)
    {
        try
        {
            await _authService.SignUpUser(userSignUpData, Request.Headers["origin"]);
            return CreatedAtAction(nameof(Register), userSignUpData);
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Error during registration. Email: {Email}, Time: {Time}, Origin: {Origin}, User Agent: {UserAgent}",
                userSignUpData.Email, DateTime.UtcNow, Request.Headers["origin"], Request.Headers.UserAgent);

            return BadRequest(new { Message = "An error occurred during registration." });
        }
    }

    /// <summary>
    /// Verify user email endpoint.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("verify-user-email")]
    public async Task<IActionResult> VerifyUserEmail([FromBody] VerifyEmailRequest verifyEmailDTO)
    {
        try
        {
            await _authService.VerifyUserEmail(verifyEmailDTO.Email, verifyEmailDTO.Token);
            return Ok("Email verification completed successfully");
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Error during email verification. Email: {Email}, Token: {Token}, Time: {Time}, User Agent: {UserAgent}",
                verifyEmailDTO.Email, verifyEmailDTO.Token, DateTime.UtcNow, Request.Headers.UserAgent);

            return BadRequest(new { Message = "An error occurred during email verification." });
        }
    }

    /// <summary>
    /// Reset password endpoint.
    /// </summary>
    [AllowAnonymous]
    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ChangePasswordRequest changePasswordRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _authService.ChangePassword(changePasswordRequest);
            return NoContent(); // 204 No Content for successful password reset
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Error during password reset. Email: {Email}, Time: {Time}, User Agent: {UserAgent}",
                changePasswordRequest.Email, DateTime.UtcNow, Request.Headers.UserAgent);

            return BadRequest(new { Message = "An error occurred during password reset." });
        }
    }

    /// <summary>
    /// Send email verification link endpoint.
    /// </summary>
    [HttpGet("verify-email-send")]
    public async Task<IActionResult> VerifyEmailSend(string email)
    {
        try
        {
            await _authService.SendVerifyEmail(email, Request.Headers["origin"]);
            return Ok("Email sent successfully");
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Error during email verification link send. Email: {Email}, Time: {Time}, Origin: {Origin}, User Agent: {UserAgent}",
                email, DateTime.UtcNow, Request.Headers.Origin, Request.Headers.UserAgent);

            return BadRequest(new { Message = "An error occurred while sending email verification." });
        }
    }

    /// <summary>
    /// Send forgot password email endpoint.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> SendForgotPasswordEmail([FromBody] ForgotPasswordRequest forgotPasswordRequest)
    {
        try
        {
            await _authService.SendForgotPasswordEmail(forgotPasswordRequest.Email, Request.Headers["origin"]);
            return NoContent(); // 204 No Content for successful email send
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Error during forgot password email send. Email: {Email}, Time: {Time}, Origin: {Origin}, User Agent: {UserAgent}",
                forgotPasswordRequest.Email, DateTime.UtcNow, Request.Headers.Origin, Request.Headers.UserAgent);

            return BadRequest(new { Message = "An error occurred while sending forgot password email." });
        }
    }

    /// <summary>
    /// Update user information endpoint.
    /// </summary>
    [HttpPut("update-user/{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] EditUserRequest editUserRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _authService.EditUser(editUserRequest);
            return NoContent(); // 204 No Content for successful user update
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Error during user update. UserId: {UserId}, Time: {Time}, User Agent: {UserAgent}",
                id, DateTime.UtcNow, Request.Headers.UserAgent);

            return BadRequest(new { Message = "An error occurred during user update." });
        }
    }

    /// <summary>
    /// Get user by GUID ID endpoint.
    /// </summary>
    [HttpGet("get-user-by-id/{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var user = await _authService.GetUserById(id);
            if (user is null)
                return NotFound();

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.Error(ex,
                "Error during fetching user by ID. UserId: {UserId}, Time: {Time}, User Agent: {UserAgent}",
                id, DateTime.UtcNow, Request.Headers.UserAgent);

            return BadRequest(new { Message = "An error occurred while fetching the user." });
        }
    }
}
