public class SMSNotification : Notification
{
    public override void Send()
    {
        Console.WriteLine("via SMS\n");
    }
}