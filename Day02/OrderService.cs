public class OrderService
{
    private readonly IPaymentProcessor _paymentProcessor;

    public OrderService(IPaymentProcessor paymentProcessor)
    {
        _paymentProcessor = paymentProcessor;
    }

    public void CheckOut(decimal amount)
    {
        _paymentProcessor.ProcessPayment(amount);
    }
}