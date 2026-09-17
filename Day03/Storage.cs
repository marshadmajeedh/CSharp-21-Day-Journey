using System.Collections.Generic;
public class Storage<T>
{
    private readonly List<T> _items;

    public Storage()
    {
        _items = [];
    }

    public void AddItem(T item)
    {
        _items.Add(item);
    }

    public void PrintAll()
    {
        foreach (T t in _items)
        {
            Console.WriteLine(
                $"Item : {t}"
            );
        }
    }
    public string TotalItemsInTheList()
    {
        return $"Total Items: {_items.Count}";
    }
}