using Microsoft.AspNetCore.Identity;

namespace Standard_Solution.Domain.Models
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
