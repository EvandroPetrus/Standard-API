using System.ComponentModel.DataAnnotations;

public class LoginRequest
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

