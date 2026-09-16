public class CardPaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine(
            $"Card payment of {amount}"
        );
    }
}