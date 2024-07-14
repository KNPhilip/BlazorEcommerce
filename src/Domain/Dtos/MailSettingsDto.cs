namespace Domain.Dtos;

/// <summary>
/// DTO representing the mail settings / config.
/// This is needed for the mail feature to work.
/// </summary>
public sealed class MailSettingsDto
{
    public static string SectionName => "MailSettings";
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required int Port { get; init; }
    public required string FromEmail { get; init; }
    public required string Host { get; init; } 
}
