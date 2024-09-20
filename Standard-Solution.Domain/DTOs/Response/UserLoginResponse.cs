using System.Text.Json.Serialization;

namespace Standard_Solution.Domain.DTOs.Response;

public class UserLoginResponse
{
    public bool Success { get; private set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Token { get; private set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? ExpireDate { get; private set; }

    public UserLoginResponse(bool success, string token, DateTime? expireDate)
    {
        Success = success;
        Token = token;
        ExpireDate = expireDate;
    }
}
