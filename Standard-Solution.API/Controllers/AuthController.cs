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

    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<ActionResult<UserLoginResponse>> Login([FromBody] UserLoginRequest userLoginData)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var resultadoLogin = await _authService.Login(userLoginData);
            return Ok(resultadoLogin);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

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
            return BadRequest(new { Message = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("verify-user-email")]
    public async Task<IActionResult> VerifyUserEmail([FromBody] VerifyEmailRequest verifyEmailDTO)
    {
        await _authService.VerifyUserEmail(verifyEmailDTO.Email, verifyEmailDTO.Token);
        return Ok("Email verification completed successfully");
    }

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
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("verify-email-send")]
    public async Task<IActionResult> VerifyEmailSend(string email)
    {
        await _authService.SendVerifyEmail(email, Request.Headers["origin"]);
        return Ok("Email sent successfully");
    }

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
            return BadRequest(new { Message = ex.Message });
        }
    }

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
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("get-user-by-id/{id}")]
    public async Task<IActionResult> GetUserById(string id)
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
            return BadRequest(new { Message = ex.Message });
        }
    }
}
