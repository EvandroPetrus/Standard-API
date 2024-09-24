using System.ComponentModel.DataAnnotations;

public class SignUpRequest
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string PasswordCheck { get; set; }
}