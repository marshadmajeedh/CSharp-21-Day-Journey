using static System.Runtime.InteropServices.JavaScript.JSType;
public record AccountProfileDto(
    Guid Id,
    string Username,
    string Email,
    DateTime CreatedAt,
    DateTime? LastLoginAt
);