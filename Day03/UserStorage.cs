public class UserStorage<T> where T : User
{
    private readonly List<T> _users;

    public UserStorage()
    {
        _users = [];
    }

    public void Add(T user)
    {
        _users.Add(user);
    }

    public void PrintNames()
    {
        foreach (T t in _users) {
            Console.WriteLine(
                $"Your name is: {t.Name}"
            );
        }
    }
}