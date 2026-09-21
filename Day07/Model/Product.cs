namespace Day07.Model;
public class Product
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; } = "Unknown";
    public decimal Price { get; set; } = 0;
    public int Stocks { get; set; } = 0;
}