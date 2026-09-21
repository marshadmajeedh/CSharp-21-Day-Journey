namespace Day07.Exceptions;
public class ProductNotFoundException(Guid id) : Exception($"Product with Id - {id} - was not found")
{
}