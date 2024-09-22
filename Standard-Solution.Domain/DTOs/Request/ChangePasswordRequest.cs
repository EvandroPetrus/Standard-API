
namespace Standard_Solution.Domain.DTOs.Request;

public class ChangePasswordRequest : UserRequestBase
{
    public string Token { get; set; }
    public string NewPassword { get; set; }

}
