public class Customer : User
{
    public int LoyaltyPoints { get; set; } = 0;

    public override void PrintUser()
    {
        base.PrintUser();
        Console.WriteLine($"Loyalty Points: {LoyaltyPoints}");
    }

    public override string Describe()
    {
        return
            $"Customer name: {Name}\n" +
            $"Customer email: {Email}\n"+
            $"Loyalty Points: {LoyaltyPoints}\n";
    }
}