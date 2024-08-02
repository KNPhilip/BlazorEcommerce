namespace Domain.Dtos;

public sealed class UserInfoDto
{
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required List<string> Roles { get; set; } = [];
}
