# Day 06 - ASP.NET Core Web API Fundamentals

## Goal

Transition from standalone C# programming into backend web development using ASP.NET Core. Design and implement a layered, RESTful CRUD API for product management using controllers, DTOs, dependency injection, service interfaces, route constraints, and structured HTTP status codes.

---

## 1. Platform Hierarchy: C# vs. .NET vs. ASP.NET Core

Understanding the relationship between the language, the runtime, and the web framework:

```text
C# (Language)
    ↓
.NET (SDK, Runtime, Base Libraries, Logging, DI Container)
    ↓
ASP.NET Core (Web Framework, HTTP Pipeline, Middleware, Routing)
    ↓
Web API (REST Endpoints & Controllers)
```

### Java to .NET Ecosystem Mapping

| Layer | Java Ecosystem | .NET Ecosystem |
|---|---|---|
| **Programming Language** | Java | C# |
| **Runtime & SDK** | JVM / JDK | Common Language Runtime (CLR) / .NET SDK |
| **Web Framework** | Spring Boot | ASP.NET Core |
| **Project Descriptor** | `pom.xml` / `build.gradle` | `.csproj` |
| **Dependency Management** | Maven / Gradle | NuGet |

---

## 2. Project Setup & Architecture

### CLI Commands
```bash
# Create a controller-based Web API project
dotnet new webapi -n Day06 --use-controllers

# Run the development server with hot reload
dotnet run
```

### Project Layout
```text
Day06/
│
├── Controllers/              # HTTP entry points (routing, binding, status codes)
│   └── ProductController.cs
│
├── DTOs/                     # API contracts decoupling internal models from clients
│   ├── RequestDto/
│   │   └── RequestProductDto.cs
│   └── ResponseDto/
│       └── ResponseProductDto.cs
│
├── Model/                    # Core domain entities
│   └── Product.cs
│
├── Services/                 # Business logic and data management
│   ├── IProductService.cs
│   └── ProductService.cs
│
├── Program.cs                # App bootstrap, DI registration, middleware pipeline
├── appsettings.json          # Configuration files
└── Day06.csproj              # Project dependencies and targets
```

---

## 3. Application Pipeline (`Program.cs`)

ASP.NET Core applications follow a strict two-stage lifecycle: **Service Registration (Builder Phase)** followed by the **Middleware Pipeline (App Phase)**.

```csharp
var builder = WebApplication.CreateBuilder(args);

// --- 1. Service Configuration (Dependency Injection) ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registering in-memory service as a Singleton
builder.Services.AddSingleton<IProductService, ProductService>();

var app = builder.Build(); // Pipeline is locked beyond this point

// --- 2. HTTP Request Pipeline (Middleware) ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### DI Registration: Why `AddSingleton` for Day 6?
- **In-Memory Store:** `ProductService` uses a private in-memory collection (`List<Product>`).
- **State Preservation:** Registering as **Singleton** guarantees a single instance of `ProductService` is shared across all incoming HTTP requests, keeping the product list intact during the application lifecycle.
- **Future Note:** When transitioning to Entity Framework Core and an external database (e.g., PostgreSQL), services and `DbContext` instances will be registered as **Scoped** (created once per HTTP request).

---

## 4. Architectural Data Flow

```text
Client HTTP Request
        │
        ▼
   [Middleware] (HTTPS, Auth, Logging)
        │
        ▼
[ProductController]
  - Model Binding converts JSON to RequestProductDto
  - Validates request boundaries
        │
        ▼
 [IProductService] ◄── Injected via Constructor
        │
        ▼
 [ProductService]
  - Manipulates internal List<Product>
  - Applies business operations
        │
        ▼
[ProductController]
  - Maps domain Product to ResponseProductDto
  - Wraps payload in semantic HTTP Status (200, 201, 400, 404)
        │
        ▼
Client HTTP Response (JSON)
```

---

## 5. Domain Models and DTOs

### Domain Entity (`Model/Product.cs`)
```csharp
namespace Day06.Model;

public class Product
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; } = "Unknown";
    public decimal Price { get; set; }
    public int Stocks { get; set; }
}
```

### Request & Response DTOs (`DTOs/`)
Records provide immutable, value-based contracts for API input and output.

```csharp
// DTOs/RequestDto/RequestProductDto.cs
namespace Day06.DTOs.RequestDto;

public record RequestProductDto(
    string Name,
    decimal Price,
    int Stocks
);

// DTOs/ResponseDto/ResponseProductDto.cs
namespace Day06.DTOs.ResponseDto;

public record ResponseProductDto(
    Guid Id,
    string Name,
    decimal Price,
    int Stocks
);
```

---

## 6. Service Layer Abstraction

### Contract (`Services/IProductService.cs`)
```csharp
using Day06.Model;

namespace Day06.Services;

public interface IProductService
{
    List<Product> GetAll();
    Product? GetById(Guid id);
    Product Add(Product product);
    Product? UpdateById(Guid id, Product product);
    Product? DeleteById(Guid id);
}
```

### Implementation (`Services/ProductService.cs`)
```csharp
using Day06.Model;

namespace Day06.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products = [];

    public List<Product> GetAll() => _products;

    public Product? GetById(Guid id) => 
        _products.FirstOrDefault(p => p.Id == id);

    public Product Add(Product product)
    {
        _products.Add(product);
        return product;
    }

    public Product? UpdateById(Guid id, Product updatedProduct)
    {
        var existingProduct = GetById(id);
        if (existingProduct is null) return null;

        existingProduct.Name = updatedProduct.Name;
        existingProduct.Price = updatedProduct.Price;
        existingProduct.Stocks = updatedProduct.Stocks;

        return existingProduct;
    }

    public Product? DeleteById(Guid id)
    {
        var existingProduct = GetById(id);
        if (existingProduct is null) return null;

        _products.Remove(existingProduct);
        return existingProduct;
    }
}
```

---

## 7. Controller Implementation (`Controllers/ProductController.cs`)

Using C# 12 Primary Constructors for clean dependency injection:

```csharp
using Day06.DTOs.RequestDto;
using Day06.DTOs.ResponseDto;
using Day06.Model;
using Day06.Services;
using Microsoft.AspNetCore.Mvc;

namespace Day06.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    // GET: api/Product
    [HttpGet]
    public IActionResult GetAll()
    {
        var response = productService.GetAll()
            .Select(p => new ResponseProductDto(p.Id, p.Name, p.Price, p.Stocks))
            .ToList();

        return Ok(response);
    }

    // GET: api/Product/{id}
    [HttpGet("{id:guid}")]
    public IActionResult GetProductById(Guid id)
    {
        var product = productService.GetById(id);
        if (product is null)
        {
            return NotFound(new { message = $"Product with ID {id} was not found." });
        }

        var response = new ResponseProductDto(product.Id, product.Name, product.Price, product.Stocks);
        return Ok(response);
    }

    // POST: api/Product
    [HttpPost]
    public IActionResult CreateProduct([FromBody] RequestProductDto productDto)
    {
        // Manual validation guards
        if (string.IsNullOrWhiteSpace(productDto.Name))
            return BadRequest(new { message = "Product name cannot be empty." });

        if (productDto.Price <= 0)
            return BadRequest(new { message = "Product price must be greater than zero." });

        if (productDto.Stocks < 0)
            return BadRequest(new { message = "Product stocks cannot be negative." });

        var newProduct = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Stocks = productDto.Stocks
        };

        var created = productService.Add(newProduct);

        var response = new ResponseProductDto(created.Id, created.Name, created.Price, created.Stocks);

        return CreatedAtAction(
            nameof(GetProductById),
            new { id = created.Id },
            response
        );
    }

    // PUT: api/Product/{id}
    [HttpPut("{id:guid}")]
    public IActionResult UpdateById(Guid id, [FromBody] RequestProductDto requestProductDto)
    {
        if (string.IsNullOrWhiteSpace(requestProductDto.Name))
            return BadRequest(new { message = "Product name cannot be empty." });

        if (requestProductDto.Price <= 0)
            return BadRequest(new { message = "Product price must be greater than zero." });

        if (requestProductDto.Stocks < 0)
            return BadRequest(new { message = "Product stocks cannot be negative." });

        var updateData = new Product
        {
            Name = requestProductDto.Name,
            Price = requestProductDto.Price,
            Stocks = requestProductDto.Stocks
        };

        var updated = productService.UpdateById(id, updateData);
        if (updated is null)
        {
            return NotFound(new { message = $"Product with ID {id} not found." });
        }

        var response = new ResponseProductDto(updated.Id, updated.Name, updated.Price, updated.Stocks);
        return Ok(response);
    }

    // DELETE: api/Product/{id}
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteById(Guid id)
    {
        var deleted = productService.DeleteById(id);
        if (deleted is null)
        {
            return NotFound(new { message = $"Product with ID {id} not found." });
        }

        var response = new ResponseProductDto(deleted.Id, deleted.Name, deleted.Price, deleted.Stocks);
        return Ok(response); // Or return NoContent();
    }
}
```

---

## 8. HTTP Status Codes & Parameter Binding

### Semantic Status Codes Reference

| Status Code | Helper Method | Scenario |
|---|---|---|
| **`200 OK`** | `Ok(payload)` | Standard successful query or update operation. |
| **`201 Created`** | `CreatedAtAction(...)` | Resource created; includes response body and `Location` header. |
| **`204 No Content`** | `NoContent()` | Successful execution returning an empty response body (common in DELETE/PUT). |
| **`400 Bad Request`** | `BadRequest(error)` | Caller sent malformed JSON or violated domain validation rules. |
| **`404 Not Found`** | `NotFound(error)` | Target resource ID does not match any existing records. |

### Source Binding Attributes
- `[FromRoute]`: Binds parameter directly from the route URL segment (e.g., `"{id:guid}"`). Route constraints like `:guid` reject non-conforming requests early with a 404.
- `[FromBody]`: Reads the HTTP request body and deserializes JSON into C# types using System.Text.Json.
- `[FromQuery]`: Binds values from URL query parameters (e.g., `?minPrice=100`).

---

## 9. Testing Flow with Swagger/OpenAPI

1. **POST `/api/Product`**: Create a new item, verify `201 Created`, copy the generated `Location` header or the `id` from the response body.
2. **GET `/api/Product`**: Retrieve all items; verify the newly created entity exists in the list.
3. **GET `/api/Product/{id}`**: Pass the GUID; verify single entity retrieval (`200 OK`).
4. **PUT `/api/Product/{id}`**: Update properties; verify changes reflected (`200 OK`).
5. **DELETE `/api/Product/{id}`**: Remove the product; verify confirmation response.
6. **GET `/api/Product/{id}`**: Query the deleted GUID; verify the API returns `404 Not Found`.

---

## Key Takeaways

- **Separation of Concerns:** Controllers handle transport-level concerns (HTTP verbs, model binding, status codes), while services encapsulate business logic and storage operations.
- **Contract Decoupling:** DTOs isolate external API contracts from internal storage models, preventing over-posting attacks and decoupling data persistence schemas from client contracts.
- **Location Transparency:** `CreatedAtAction` constructs RFC-compliant `Location` headers pointing to newly created resources dynamically without hardcoding URLs.