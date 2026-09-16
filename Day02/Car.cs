public class Car
{
    private readonly IEngine _engine;

    public Car(IEngine engine)
    {
        _engine = engine;
    }

    public void StartCar()
    {
        Console.WriteLine("Car starting...");
        _engine.Start();
    }

    public void StopCar()
    {
        Console.WriteLine("Car stopping...");
        _engine.Stop();
    }
}