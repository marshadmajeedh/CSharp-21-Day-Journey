using System.Dynamic;

public class Account
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}