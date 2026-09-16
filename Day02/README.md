# Day 02 - OOP, Interfaces and Dependency Injection

## Goal

Understand core C# Object-Oriented Programming (OOP) concepts, interface abstractions, and dependency injection patterns commonly used in ASP.NET Core backend development.

---

## Topics Learned

### 1. Inheritance & The IS-A Relationship
Inheritance models an **IS-A** relationship where derived classes specialize a common base class:
- `Staff` IS-A `User`
- `Customer` IS-A `User`

In C#, the colon (`:`) syntax is used for both class extension and interface implementation:

```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class Staff : User
{
    public string Role { get; set; } = string.Empty;
}
```

### 2. Virtual, Override & Runtime Polymorphism
C# requires explicit intent when overriding base class behavior:
- Base class marks a method as `virtual` to allow overrides.
- Derived class explicitly uses the `override` keyword.

```csharp
public class User
{
    public virtual string Describe() => "Standard User";
}

public class Customer : User
{
    public override string Describe() => "Customer Account";
}

public class Staff : User
{
    public override string Describe() => "Staff Member";
}
```

**Runtime Polymorphism in action:**
```csharp
List<User> users =
[
    new User(),
    new Customer(),
    new Staff()
];

foreach (var user in users)
{
    // Executes the implementation belonging to the actual runtime type
    Console.WriteLine(user.Describe());
}
```

### 3. Abstract Classes
Abstract classes define a template that cannot be directly instantiated. They allow sharing state and concrete implementation alongside abstract method contracts:

```csharp
public abstract class Notification
{
    public string Recipient { get; set; } = string.Empty;

    public void PrintRecipient()
    {
        Console.WriteLine($"Recipient: {Recipient}");
    }

    public abstract void Send();
}

public class EmailNotification : Notification
{
    public override void Send()
    {
        Console.WriteLine($"Sending email to {Recipient}");
    }
}
```

### 4. Interfaces
Interfaces define pure behavior contracts without state. Unlike classes, a C# class can implement multiple interfaces:

```csharp
public interface IPaymentProcessor
{
    void ProcessPayment(decimal amount);
}

public class CardPaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount) =>
        Console.WriteLine($"Processing card payment of {amount:C}");
}

public class CashPaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount) =>
        Console.WriteLine($"Processing cash payment of {amount:C}");
}
```

### 5. Dependency Injection (Constructor Injection)
High-level classes receive their dependencies via constructors rather than instantiating them directly with `new`:

```csharp
public class OrderService
{
    private readonly IPaymentProcessor _paymentProcessor;

    public OrderService(IPaymentProcessor paymentProcessor)
    {
        _paymentProcessor = paymentProcessor;
    }

    public void CompleteOrder(decimal total)
    {
        _paymentProcessor.ProcessPayment(total);
    }
}
```
*Benefit:* `OrderService` is completely decoupled from concrete payment logic, enabling easy swapping of implementations and straightforward unit testing with mocks.

### 6. Composition over Inheritance (HAS-A vs. IS-A)
Prefer composition (`HAS-A`) when behavior needs to be swapped dynamically or independently of the class hierarchy:

```csharp
public interface IEngine
{
    void Start();
}

public class PetrolEngine : IEngine
{
    public void Start() => Console.WriteLine("Vroom (Petrol Engine)");
}

public class ElectricMotor : IEngine
{
    public void Start() => Console.WriteLine("Silent hum (Electric Motor)");
}

public class DieselEngine : IEngine
{
    public void Start() => Console.WriteLine("Rumble (Diesel Engine)");
}

public class Car
{
    private readonly IEngine _engine;

    // Car HAS-A Engine
    public Car(IEngine engine)
    {
        _engine = engine;
    }

    public void Drive()
    {
        _engine.Start();
    }
}
```

---

## ASP.NET Core DI Lifetimes

| Lifetime | Meaning | Common Use Case |
|---|---|---|
| **Transient** | Created every time an instance is requested. | Lightweight, stateless services or utilities. |
| **Scoped** | Created once per HTTP request pipeline. | Database contexts (`DbContext`), repositories, unit of work. |
| **Singleton** | Created once on first request and shared application-wide. | Caching services, configurations, background workers. |

---

## Core Design Principles

- **Dependency Inversion Principle (DIP):** High-level modules (`OrderService`, `Car`) should not depend on low-level modules (`CardPaymentProcessor`, `PetrolEngine`). Both should depend on abstractions (`IPaymentProcessor`, `IEngine`).
- **Open/Closed Principle (OCP):** Software entities should be open for extension, but closed for modification. New engine types (e.g., `DieselEngine`) or payment channels can be introduced without altering existing classes.

---

## Java vs. C# Quick Reference

| Concept | Java | C# |
|---|---|---|
| **Inheritance & Interface Syntax** | `class B extends A implements I` | `class B : A, I` |
| **Method Overriding** | Implicitly virtual; `@Override` annotation | Explicit: `virtual` in base, `override` in derived |
| **Interface Naming** | Conventional (often no prefix) | Standard `I` prefix (`IPaymentProcessor`) |
| **Collection Expressions** | `List.of(...)` / `Arrays.asList(...)` | `[...]` (C# 12 collection expressions) |
| **Dependency Injection** | Spring `@Autowired` / constructor injection | Native built-in DI container (`IServiceCollection`) |