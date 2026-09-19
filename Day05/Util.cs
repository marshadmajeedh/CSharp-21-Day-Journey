public class Util
{
    public static async Task SimulateDownloadAsync()
    {
        Console.WriteLine("Download started");
        await Task.Delay(3000);
        Console.WriteLine("Download completed");
    }

    public static async Task<string> GetUsernameAsync()
    {
        await Task.Delay(2000);
        return "Marshad";
    }

    //Day 5 first exercise
    public static async Task PrintMessageAsync()
    {
        Console.WriteLine("Processing...");
        await Task.Delay(2000);
        Console.WriteLine("Processing completed\n");
    }
    public static async Task<int> CalculateAsync(int number)
    {
        await Task.Delay(1000);
        return number * 2;
    }
    public static async Task<string> GetProductNameAsync(long id)
    {
        await Task.Delay(1500);

        if (id == 1)
        {
            return "Mechanical keyboard";
        }
        else
        {
            return "Product not found";
        }
    }
    public static async Task<string> GetOrderAsync(int id)
    {
        await Task.Delay(2000);

        if(id <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.");
        }
        return $"Order id: {id}";
    }
    public static async Task<decimal> ProcessPaymentAsync(decimal amount)
    {
        await Task.Delay(1500);

        if (amount <= 0)
        {
            throw new ArgumentException($"Payment amount must be greater than zero to proceed");
        }

        return amount;
    }
    public static decimal CalculateOrderTotal(decimal price, int quantity)
    {
        return price * quantity;
    }
    public static async Task<string> SaveOrderAsync(int orderId,decimal total)
    {
        await Task.Delay(1500);

        return $"Order id: {orderId}, total cost: {total}";
    }
}
