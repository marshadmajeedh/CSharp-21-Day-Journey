using Microsoft.AspNetCore.Diagnostics;

namespace Day08.Exceptions;

public class ProductNotFoundException(Guid id) : Exception($"Product with Id - {id} was not found");