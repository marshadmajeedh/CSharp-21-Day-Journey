public abstract class Notification
{
    public string Recipient { get; set; } = "Unknown";

    public void PrintRecipient()
    {
        Console.Write(
            $"Sending to: {Recipient} "
        );
    }

    public abstract void Send();
}