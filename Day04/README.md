# Day 04 - Modern C# Features

## Goal

Master modern C# language features and idioms commonly used across .NET backend services and ASP.NET Core applications, focusing on records, pattern matching, null safety, and property encapsulation.

---

## Topics Learned

### 1. Records & Value-Based Equality
A `record` is a reference type designed primarily for immutable data storage. Unlike standard classes that compare references (memory addresses), records use **value-based equality**.

```csharp
public record ProductDto(long Id, string Name, decimal Price);

// Instantiation (C# 9+ target-typed new)
ProductDto product1 = new(1, "Mechanical Keyboard", 100.00m);
ProductDto product2 = new(1, "Mechanical Keyboard", 100.00m);

// Value-based equality check
Console.WriteLine(product1 == product2); // Output: True

// Built-in formatted string representation
Console.WriteLine(product1); 
// Output: ProductDto { Id = 1, Name = Mechanical Keyboard, Price = 100.00 }
```

### 2. Non-Destructive Mutation (`with` Expressions)
Records support non-destructive mutation via `with` expressions, creating a shallow copy with specified property updates while leaving the original instance untouched:

```csharp
ProductDto updatedProduct = product1 with 
{ 
    Price = 120.00m 
};

// product1.Price is still 100.00m
// updatedProduct.Price is 120.00m
```

---

### 3. Pattern Matching & Switch Expressions
Modern C# allows matching against types, object properties, values, and relational ranges cleanly without cascading `if`/`else` trees.

#### Property & Relational Patterns
```csharp
if (employee is { Age: >= 18, Salary: > 50000m })
{
    Console.WriteLine("Eligible for senior benefit package.");
}
```

#### Switch Expressions with Logical Combinators (`and`, `or`, `not`)
```csharp
string ageCategory = employee.Age switch
{
    >= 18 and <= 25 => "Young adult",
    >= 26 and <= 40 => "Adult",
    >= 41 => "Senior",
    _ => "Minor" // Discard arm (catch-all default)
};
```

---

### 4. Tuples & Deconstruction
Tuples provide a lightweight way to return multiple strongly typed values from a method without declaring a dedicated class or struct.

```csharp
// Method returning a named tuple
public static (int Min, int Max) GetMinMax(List<int> numbers)
{
    return (numbers.Min(), numbers.Max());
}

// Consuming and deconstructing into distinct variables
List<int> values = [12, 5, 99, 23, 1];
var (minimum, maximum) = GetMinMax(values);

Console.WriteLine($"Min: {minimum}, Max: {maximum}"); // Min: 1, Max: 99
```

---

### 5. Nullable Reference Types (NRT) & Null Safety
When enabled, C# treats reference types as non-nullable by default, flagging unhandled null possibilities at compile time.

```csharp
public class UserProfile
{
    public string Username { get; set; } = string.Empty; // Non-nullable (cannot be null)
    public string? Bio { get; set; }                     // Explicitly nullable
}
```

#### Null-Safe Operators
```csharp
// 1. Safe navigation (?.): Executes only if Bio is not null
string? upperBio = user.Bio?.ToUpper();

// 2. Null-coalescing (??): Supplies a fallback value
string displayBio = user.Bio?.ToUpper() ?? "Bio is unavailable";

// 3. Modern null check and throw (C# 10+)
public static int GetBioLength(string? bio)
{
    ArgumentNullException.ThrowIfNull(bio);
    return bio.Length;
}
```

---

### 6. Property Modifiers & Encapsulation

| Modifier / Pattern | Allowed Changes | Caller Obligation | Typical Use Case |
|---|---|---|---|
| `public string P { get; set; }` | Mutable anytime | Optional during `new` | Mutable entity state |
| `public required string P { get; set; }` | Mutable anytime | **Mandatory** during `new` | Required mutable fields |
| `public required string P { get; init; }` | **Locked** after initialization | **Mandatory** during `new` | API request contracts / DTOs |
| `public string P { get; private set; }` | Class methods only | Optional during `new` | Domain models with controlled mutations |
| `private readonly string _p;` | Constructor only | Class constructor logic | Injected service dependencies |

#### Code Comparison:
```csharp
public class Account
{
    // Mandatory assignment via object initializer; cannot mutate afterward
    public required string Username { get; init; }

    // Read by anyone; modified ONLY by methods inside this class
    public DateTime? LastLoginAt { get; private set; }

    public void MarkLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }
}

// Instantiation:
var acc = new Account { Username = "Ahamed" }; // Valid
// acc.Username = "NewName";                  // ❌ Compiler Error (init-only)
// acc.LastLoginAt = DateTime.UtcNow;         // ❌ Compiler Error (private set)
acc.MarkLogin();                              // ✅ Handled via domain method
```

---

### 7. Shallow Immutability Caveat
`init` protects the property reference, but not the contents of a mutable collection it holds:

```csharp
public class User
{
    public required List<string> Roles { get; init; }
}

var user = new User { Roles = ["Admin"] };

// user.Roles = new List<string>(); // ❌ Blocked: Reference cannot be reassigned
user.Roles.Add("SuperAdmin");       // ⚠️ Allowed: Internal list contents still mutate!
```
*Takeaway:* An immutable reference does not guarantee an immutable object graph. To enforce complete immutability, expose `IReadOnlyCollection<T>` or `ImmutableList<T>`.

---

### 8. Domain Entities vs. DTO Records

```text
Database / Persistence
        ↓
Domain Entity (Class: Encapsulated logic, private setters, business guards)
        ↓
Application Service
        ↓
Data Transfer Object (Record: Flat, immutable value contracts)
        ↓
API Controller → JSON Response → Client
```

- **Domain Entity (`AccountProfile`):** Contains business invariants, behaviors, and controlled lifecycle transitions (`account.MarkLogin()`).
- **DTO Record (`AccountDto`):** Thin, immutable data carrier optimized for serialization over HTTP.

---

## Java to C# Quick Reference

| Feature | Java | C# |
|---|---|---|
| **Immutable Value Carrier** | `record Point(int x, int y)` | `public record Point(int X, int Y);` |
| **Non-Destructive Copying** | Manual builder / constructor copy | Built-in `with` expression |
| **Read-Only Fields** | `final` field | `readonly` field / `{ get; init; }` |
| **Mandatory Initialization** | Constructor parameters / `@NonNull` | `required` keyword |
| **Nullable Intent** | Optional / `@Nullable` annotations | Native `T?` syntax with compiler analysis |
| **Null-Safe Throw** | `Objects.requireNonNull(val)` | `ArgumentNullException.ThrowIfNull(val)` |
| **Encapsulation Access** | Boilerplate getters / private setters | Properties (`{ get; private set; }`) |

---

## Key Takeaways

- **Records** drastically reduce DTO boilerplate, giving value equality, decomposition, and formatted output out of the box.
- **`with` expressions** make working with immutable records efficient by eliminating manual cloning logic.
- **`required` and `init`** combine the safety of immutable fields with the clean readability of object initializers.
- **Pattern matching** treats type checks, range checks, and property checks as first-class control flow operations.