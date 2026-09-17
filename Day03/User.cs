public class User
{
    public long Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public virtual void PrintUser()
    {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Email: {Email}");
    }
    public virtual string Describe()
    {
        return $"User: {Name}\n" +
            $"Email: {Email}\n";
    }
}