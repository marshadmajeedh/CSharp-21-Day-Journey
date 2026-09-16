public class ElectricMotor : IEngine{
    public void Start()
    {
        Console.WriteLine(
            "Electric Motor Engine started\n"
        );
    }

    public void Stop()
    {
        Console.WriteLine(
            "Electric Motor Engine stopped\n"
        );
    }
}