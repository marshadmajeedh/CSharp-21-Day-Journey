public class Staff : User
{
    public string Department { get; set; } = "";

    public override string Describe()
    {
        return
            $"Staff name: {Name}\n"+
            $"Staff email: {Email}\n"+
            $"Department: {Department}\n";
    }

    public override void PrintUser()
    {
        base.PrintUser();
        Console.WriteLine($"Department: {Department}");
    }
}