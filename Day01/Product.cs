public class Product
{
    private static long _nextId = 1;
    public long Id { get; }
    public string Name { get; private set; }
    public decimal Price { get; private set;  }
    public int Stocks { get; private set; }

    public ProductStatus Status() => Stocks switch
    {
        < 1 => ProductStatus.OutOfStock,
        < 10 => ProductStatus.LowStock,
        _ => ProductStatus.Available
    };

    public Product(string name, decimal price, int stocks)
    {
        SetName(name);
        SetPrice(price);
        SetStocks(stocks);
        Id = _nextId++;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty");
        }
        Name = name;
    }
    public void SetPrice(decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentException("Price cannot be negative value");
        }
        Price = price;
    }
    public void SetStocks(int stocks)
    {
        if (stocks < 0)
        {
            throw new ArgumentException("Stocks cannot be negative");
        }
        Stocks = stocks;
    }
    public bool CanFulfill(int quantity){
        return Stocks >= quantity;
    }
    public void ReduceStock(int quantity)
    {

        if (quantity < 1)
        {
            throw new ArgumentException("Quantity cannot be less than 1");
        }

        if (!CanFulfill(quantity))
        {
            Console.WriteLine("Sorry, Current available stocks are not sufficient to full fill your required stocks");
            return;
        }
        Stocks -= quantity;
    }

    public void PrintDetails(){
        Console.WriteLine($"Product name : {Name}");
        Console.WriteLine($"Product id: {Id}");
        Console.WriteLine($"Product price : {Price}");
        Console.WriteLine($"Product stock : {Stocks}");
    }

}
