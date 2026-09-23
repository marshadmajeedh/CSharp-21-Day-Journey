namespace Day08.Model;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int Stocks{ get; set; }
}