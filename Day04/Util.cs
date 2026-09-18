public class Util
{
    public static (int Min, int Max) GetMinMax(List<int> numbers)
    {
        int min = numbers.Min();
        int max = numbers.Max();

        return (min, max);
    }

    public static void PrintBio(string? bio)
    {
        if (bio is null)
        {
            Console.WriteLine("Bio cannot be null\n");
            return;
        }

        Console.WriteLine(
            "Your profile bio is : " + bio.ToUpper() + "\n"
        );
    }
    public static int GetBioLength(string? bio)
    {
        ArgumentNullException.ThrowIfNull(bio);

        return bio.Length;
    }
}