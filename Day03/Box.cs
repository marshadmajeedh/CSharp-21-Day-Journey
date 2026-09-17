public class Box<T>
{
    public T? Value { get; set; }

    public void PrintValue()
    {
        Console.WriteLine(
            $"Printed value: {Value}"
        );
    }
}