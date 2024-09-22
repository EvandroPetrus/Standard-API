using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Standard_Solution.Domain.DTOs.Request;
using Standard_Solution.Domain.DTOs.Response;
using Standard_Solution.Domain.Interfaces.Services;

namespace Standard_Solution.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

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
            return Ok(loginResult);
        }
        catch (Exception ex)
        {
            // Log the exception (consider using a logging framework)
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// User registration endpoint.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] UserSignUpRequest userSignUpData)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _authService.SignUpUser(userSignUpData, Request.Headers["origin"]);
            return CreatedAtAction(nameof(Register), userSignUpData);
        }
        catch (Exception ex)
        {
            // Log the exception (consider using a logging framework)
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Verify user email endpoint.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("verify-user-email")]
    public async Task<IActionResult> VerifyUserEmail([FromBody] VerifyEmailRequest verifyEmailDTO)
    {
        await _authService.VerifyUserEmail(verifyEmailDTO.Email, verifyEmailDTO.Token);
        return Ok("Email verification completed successfully");
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
            // Log the exception (consider using a logging framework)
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Send email verification link endpoint.
    /// </summary>
    [HttpGet("verify-email-send")]
    public async Task<IActionResult> VerifyEmailSend(string email)
    {
        await _authService.SendVerifyEmail(email, Request.Headers["origin"]);
        return Ok("Email sent successfully");
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
            // Log the exception (consider using a logging framework)
            return BadRequest(new { Message = ex.Message });
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
            // Log the exception (consider using a logging framework)
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Get user by ID endpoint.
    /// </summary>
    [HttpGet("get-user-by-id/{id}")]
    public async Task<IActionResult> GetUserById(int id)
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
            // Log the exception (consider using a logging framework)
            return BadRequest(new { Message = ex.Message });
        }
    }
}
