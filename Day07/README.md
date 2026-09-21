# Day 07: Validation, Logging & Global Exception Handling

## Overview

Day 07 refactors the Day 06 ASP.NET Core CRUD API to follow clean architecture principles and the Single Responsibility Principle (SRP). By moving validation to Data Annotations, routing errors through a centralized global exception pipeline (`IExceptionHandler`), and adopting structured logging via `ILogger<T>`, controller actions are kept lightweight and focused solely on HTTP coordination.

---

## 1. Request Pipeline Architecture

```text
HTTP Request
     │
     ▼
Model Binding
     │
     ▼
Data Annotation Validation  ──(Invalid)──►  400 Bad Request (Automatic via [ApiController])
     │ (Valid)
     ▼
Controller
     │
     ▼
Service Layer
     │
     ├───► [Success] ────────► 200 OK / 201 Created (Response DTO)
     │
     └───► [Domain Failure] ─► Throws Custom Exception (e.g., ProductNotFoundException)
                                     │
                                     ▼
                              Global Exception Handler (IExceptionHandler)
                                     │
                                     ▼
                              ProblemDetails Response (RFC 7807)
                                (404 Not Found / 500 Internal Server Error)
```

---

## 2. Request Validation via Data Annotations

Validation logic is decoupled from controller actions and placed directly onto request DTOs.

### DTO Definition

```csharp
using System.ComponentModel.DataAnnotations;

namespace Day07.DTOs.RequestDto;

public class RequestProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must contain between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int Stocks { get; set; }
}
```

### Validation Rules Summary

| Attribute | Constraint | Failure Condition |
| :--- | :--- | :--- |
| `[Required]` | Value must be supplied | Null, empty string, or missing key |
| `[StringLength(100, MinimumLength = 2)]` | 2 to 100 characters | 0–1 chars or > 100 chars |
| `[Range(0.01, double.MaxValue)]` | Price > 0 | Price <= 0 |
| `[Range(0, int.MaxValue)]` | Stock >= 0 | Stock < 0 |

### Automatic Validation with `[ApiController]`

Applying `[ApiController]` to the controller enables automatic model validation before the action executes:

```text
Request ──► Model Binding ──► Validation Failed ──► 400 Bad Request (Stops Execution)
```

Manual checks like `if (!ModelState.IsValid)` or individual property checks inside controller actions are eliminated.

---

## 3. Structured Logging with `ILogger<T>`

ASP.NET Core's built-in `ILogger<T>` is injected via dependency injection.

### Controller Injection

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductController(
    IProductService productService,
    ILogger<ProductController> logger
) : ControllerBase
{
    private readonly IProductService _productService = productService;
    private readonly ILogger<ProductController> _logger = logger;
}
```

### Log Levels

* **`LogInformation`**: Standard application telemetry (e.g., successful entity creation, endpoint entry).
* **`LogWarning`**: Predictable failures or unfulfilled conditions (e.g., missing entity queries).
* **`LogError`**: Runtime faults, database connectivity issues, or unhandled exceptions.

### Structured Logging vs. String Interpolation

Always use message templates with named parameters instead of string interpolation:

```csharp
// BAD: String interpolation (defeats structured indexing and searchability)
_logger.LogInformation($"Product {product.Id} created");

// GOOD: Structured logging template (preserves properties in queryable log sinks)
_logger.LogInformation("Product {ProductId} created successfully", product.Id);
```

> **Security Rule:** Never log sensitive parameters (passwords, JWTs, API keys, payment information, or PII).

---

## 4. Custom Domain Exceptions

Instead of returning `null` when a resource is absent, throw strongly typed domain exceptions.

### `ProductNotFoundException.cs`

```csharp
namespace Day07.Exceptions;

public class ProductNotFoundException(Guid id)
    : Exception($"Product with Id '{id}' was not found.")
{
}
```

---

## 5. Refactoring the Service Layer

Because domain failures throw exceptions immediately, methods guarantee a non-null return value on success, eliminating redundant null-checking across operations.

```csharp
public class ProductService : IProductService
{
    private readonly List<Product> _products = [];

    public Product GetById(Guid id)
    {
        return _products.FirstOrDefault(p => p.Id == id)
            ?? throw new ProductNotFoundException(id);
    }

    public Product UpdateById(Guid id, Product updatedProduct)
    {
        // GetById throws ProductNotFoundException if missing; no null-check required
        Product product = GetById(id);

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Stocks = updatedProduct.Stocks;

        return product;
    }

    public Product DeleteById(Guid id)
    {
        Product product = GetById(id);
        _products.Remove(product);
        return product;
    }
}
```

---

## 6. Global Exception Handling (`IExceptionHandler`)

All unhandled exceptions are caught centrally and translated into standard RFC 7807 `ProblemDetails` responses.

### `GlobalExceptionHandler.cs`

```csharp
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Day07.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An exception occurred: {Message}", exception.Message);

        ProblemDetails problemDetails = exception switch
        {
            ProductNotFoundException notFoundEx => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Product Not Found",
                Detail = notFoundEx.Message,
                Type = "[https://tools.ietf.org/html/rfc7231#section-6.5.4](https://tools.ietf.org/html/rfc7231#section-6.5.4)"
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred. Please try again later.",
                Type = "[https://tools.ietf.org/html/rfc7231#section-6.6.1](https://tools.ietf.org/html/rfc7231#section-6.6.1)"
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
```

### Registration in `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register Global Exception Handling & ProblemDetails
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();

var app = builder.Build();

// Activate Exception Handling Middleware early in the pipeline
app.UseExceptionHandler();

app.UseAuthorization();
app.MapControllers();
app.Run();
```

---

## 7. Cleaned Controller Endpoints

With validation handled by attributes and error routing handled by `IExceptionHandler`, the controller focuses solely on HTTP orchestration.

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductController(
    IProductService productService,
    ILogger<ProductController> logger
) : ControllerBase
{
    private readonly IProductService _productService = productService;
    private readonly ILogger<ProductController> _logger = logger;

    [HttpGet("{id:guid}")]
    public IActionResult GetProductById(Guid id)
    {
        _logger.LogInformation("Fetching product with ID: {ProductId}", id);
        
        Product product = _productService.GetById(id);
        
        ResponseProductDto response = new(product.Id, product.Name, product.Price, product.Stocks);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateProduct(Guid id, RequestProductDto requestProductDto)
    {
        _logger.LogInformation("Updating product with ID: {ProductId}", id);

        Product updatedProduct = new()
        {
            Name = requestProductDto.Name,
            Price = requestProductDto.Price,
            Stocks = requestProductDto.Stocks
        };

        Product result = _productService.UpdateById(id, updatedProduct);
        return Ok(new ResponseProductDto(result.Id, result.Name, result.Price, result.Stocks));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteProduct(Guid id)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", id);

        Product deletedProduct = _productService.DeleteById(id);
        return Ok(new ResponseProductDto(deletedProduct.Id, deletedProduct.Name, deletedProduct.Price, deletedProduct.Stocks));
    }
}
```

---

## 8. API Verification Scenarios

### 1. Invalid Payload (Validation Trigger)

* **Request:** `POST /api/Product`
  ```json
  {
    "name": "",
    "price": -50.0,
    "stocks": -3
  }
  ```
* **Status:** `400 Bad Request`
* **Response Body:** Handled by ASP.NET Core model validation with field-specific error details.

### 2. Entity Not Found

* **Request:** `GET /api/Product/11111111-1111-1111-1111-111111111111`
* **Status:** `404 Not Found`
* **Response Body:**
  ```json
  {
    "type": "[https://tools.ietf.org/html/rfc7231#section-6.5.4](https://tools.ietf.org/html/rfc7231#section-6.5.4)",
    "title": "Product Not Found",
    "status": 404,
    "detail": "Product with Id '11111111-1111-1111-1111-111111111111' was not found."
  }
  ```

### 3. Successful Mutation

* **Request:** `PUT /api/Product/{valid-guid}`
* **Status:** `200 OK`
* **Response Body:** Updated product DTO.

---

## 9. Day 06 vs. Day 07 Architecture Comparison

| Area | Day 06 Approach | Day 07 Approach |
| :--- | :--- | :--- |
| **Model Validation** | Manual `string.IsNullOrWhiteSpace()` and `if (x <= 0)` blocks per controller action | Declarative Data Annotations (`[Required]`, `[Range]`) with automatic rejection via `[ApiController]` |
| **Missing Records** | Service returned `null`; controllers performed repetitive `if (res is null) return NotFound();` | Service throws `ProductNotFoundException`; captured globally |
| **Exception Handling** | Local `try/catch` or raw unhandled server 500 HTML screens | Centralized `IExceptionHandler` returning RFC 7807 `ProblemDetails` |
| **Logging** | Raw string interpolation (`$"..."`) | Structured templates with `ILogger<T>` (`"{Property}"`) |
| **Controller Scope** | Mixed: HTTP + Input Checks + Error Mapping | Lean orchestration: HTTP mapping and delegation only |