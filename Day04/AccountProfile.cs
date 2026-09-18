public class AccountProfile
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string Username { get; init; }
    public required string Email { get; init; }
    public DateTime CreatedAt { get; } = DateTime.Now;
    public DateTime? LastLoginAt { get; private set; }
    public void MarkLogin()
    {
        LastLoginAt = DateTime.Now;
    }
}