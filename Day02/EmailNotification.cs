public class EmailNotification : Notification{
    public override void Send()
    {
        Console.WriteLine("via Email\n");
    }
}