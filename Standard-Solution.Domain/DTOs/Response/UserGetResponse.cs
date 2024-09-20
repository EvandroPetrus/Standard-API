namespace Standard_Solution.Domain.DTOs.Response;

public class UserGetResponse
{
    public int Id { get; set; }
    public string Email { get; set; }
    public DateTime? ExpireDate { get; set; }
}
