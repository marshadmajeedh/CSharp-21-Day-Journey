public class PetrolEngine : IEngine
{
    public void Start()
    {
        Console.WriteLine(
            "Petrol Engine started\n"
        );
    }

    public void Stop()
    {
        Console.WriteLine(
            "Petrol Engine stopped\n"
        );
    }

}