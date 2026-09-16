# Day 01 - C# Fundamentals

## Goal

Start transitioning from Java to C# and understand the core C# features needed before learning ASP.NET Core.

---

## Topics Learned

### C# Basics
- Variables and strongly typed data
- `var` type inference
- `decimal` for monetary values
- String interpolation
- Modern C# object creation

### Classes and Objects
- Classes
- Objects
- Constructors
- Properties
- `get`
- `set`
- `private set`
- Static fields

### Encapsulation
Used properties such as:

```csharp
public decimal Price { get; private set; }
```

This allows other classes to read the price while preventing them from directly modifying it.

Business rules are handled through methods such as:
- `SetPrice(...)`
- `SetStocks(...)`
- `ReduceStock(...)`

### Collections
Learned how to use:
- `List<Product>`

Iterated using:
- `foreach`

### LINQ
Practiced:
- `Where()`
- `Sum()`
- `FirstOrDefault()`

Example:
```csharp
var products = productList.Where(p => p.Stocks >= 10);
```

### Lambda Expressions
Example:
```csharp
p => p.Stocks >= 10
```

### Null Handling
Learned:
- `?.`
- `??`
- `??=`

Example:
```csharp
string productName = product?.Name ?? "Product not found";
```

### Enums and Switch Expressions
Created a `ProductStatus` enum:
```csharp
public enum ProductStatus
{
    Available,
    LowStock,
    OutOfStock
}
```

Used a switch expression:
```csharp
public ProductStatus Status => Stocks switch
{
    0 => ProductStatus.OutOfStock,
    < 10 => ProductStatus.LowStock,
    _ => ProductStatus.Available
};
```

### Exception Handling
Practiced:
- `throw`
- `try`
- `catch`

Validation was added for:
- Empty product name
- Negative price
- Negative stock
- Invalid stock reduction

---

## Java to C# Comparisons

| Java | C# |
|---|---|
| `boolean` | `bool` |
| `String` | `string` |
| Getter/setter methods | Properties |
| `ArrayList<T>` / `List<T>` | `List<T>` |
| Streams | LINQ |
| `p -> condition` | `p => condition` |
| JVM / JDK | .NET |
| Spring Boot | ASP.NET Core |

---

## Mini Project

Created a simple product inventory system supporting:
- Product creation
- Automatic local IDs
- Product validation
- Stock management
- Product status
- Product searching
- Inventory value calculation
- LINQ queries
- Exception handling

---

## Key Takeaway

The biggest difference from Java today was C# properties and modern syntax. Concepts such as OOP, exceptions, collections, and interfaces are familiar from Java, but C# provides cleaner syntax for many of them.