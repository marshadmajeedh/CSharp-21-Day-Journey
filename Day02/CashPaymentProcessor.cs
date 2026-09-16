public class CashPaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine(
            $"Cash payment of {amount}"
        );
    }
}