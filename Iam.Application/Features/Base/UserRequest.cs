using System.Text.Json.Serialization;

namespace Iam.Application.Features.Base;

public record UserRequest
{
    [JsonIgnore]
    public int UserId { get; set; }
    [JsonIgnore]
    public string? UserName { get; set; } = string.Empty;
}
