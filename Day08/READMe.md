Day 08 — Entity Framework Core Fundamentals

Overview

Day 08 replaces the temporary in-memory List<Product> storage from earlier days with a real SQLite database using Entity Framework Core.

The application now follows this flow:

HTTP Request
    ↓
ProductController
    ↓
IProductService
    ↓
ProductService
    ↓
AppDbContext
    ↓
DbSet<Product>
    ↓
Entity Framework Core
    ↓
SQLite

The main goal was to understand how EF Core maps C# entities to database tables, how migrations create and update the schema, how dependency injection works with DbContext, and how asynchronous CRUD operations are performed.

Technologies Used

C#

.NET 8

ASP.NET Core Web API

Entity Framework Core 8

SQLite

Swagger / OpenAPI

NuGet Packages

dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0

Because the project targets .NET 8, EF Core 8 is used.

Product Entity

namespace Day08.Model;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";

    public decimal Price { get; set; }

    public int Stocks { get; set; }
}

EF Core recognizes Id as the primary key by convention.

Common primary-key conventions include:

Id
<EntityName>Id

For example:

Id
ProductId

AppDbContext

AppDbContext acts as the bridge between the application and the database.

using Day08.Model;
using Microsoft.EntityFrameworkCore;

namespace Day08.Data;

public class AppDbContext(
    DbContextOptions<AppDbContext> options
) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}

DbSet<Product> represents the set of Product entities that EF Core maps to the Products table.

Connection String

The SQLite connection string is stored in appsettings.json.

{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=day08.db"
  }
}

For SQLite, day08.db is the database file stored on disk.

Registering AppDbContext

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"
        )
    )
);

This registration does three things:

AddDbContext<AppDbContext>
→ registers AppDbContext with dependency injection

UseSqlite
→ configures EF Core to use SQLite

GetConnectionString("DefaultConnection")
→ reads the database connection string from configuration

Service Lifetime

Earlier versions used:

builder.Services.AddSingleton<IProductService, ProductService>();

That was useful while the service itself owned an in-memory List<Product>, because the same service instance had to survive between requests.

After introducing EF Core:

builder.Services.AddScoped<IProductService, ProductService>();

AddDbContext<AppDbContext>() registers the DbContext as scoped by default.

The service is therefore also scoped.

One HTTP Request
    ↓
One ProductService
    ↓
One AppDbContext

The data no longer needs to live inside the service because SQLite provides persistent storage.

EF Core Migrations

A migration records database schema changes based on the C# entity model.

Create a migration:

dotnet ef migrations add InitialCreate

Apply migrations:

dotnet ef database update

List migrations:

dotnet ef migrations list

The initial migration created:

Products
__EFMigrationsHistory

__EFMigrationsHistory stores which migrations have already been applied.

EF Core checks this table before applying future migrations.

Generated Products Table

The SQLite provider generated SQL similar to:

CREATE TABLE "Products" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Products" PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Price" TEXT NOT NULL,
    "Stocks" INTEGER NOT NULL
);

This demonstrates the ORM mapping:

C# Product entity
    ↓
EF Core model
    ↓
Migration
    ↓
SQLite Products table

ProductService with EF Core

Read All Products

public async Task<IEnumerable<Product>> GetAllAsync()
{
    return await _context.Products
        .AsNoTracking()
        .ToListAsync();
}

AsNoTracking() is useful for read-only queries because EF Core does not need to track changes to the returned entities.

Add Product

public async Task<Product> AddProductAsync(Product addedProduct)
{
    _context.Products.Add(addedProduct);

    await _context.SaveChangesAsync();

    return addedProduct;
}

Add() marks the entity as Added inside the DbContext.

SaveChangesAsync() generates and sends the required INSERT SQL to the database.

Get Product by ID

public async Task<Product> GetByIdAsync(Guid id)
{
    Product product =
        await _context.Products
            .FirstOrDefaultAsync(product => product.Id == id)
        ?? throw new ProductNotFoundException(id);

    return product;
}

This query remains tracked because the retrieved product may later be updated or deleted.

Update Product

public async Task<Product> UpdateByIdAsync(
    Guid id,
    Product updatedProduct)
{
    Product product = await GetByIdAsync(id);

    product.Name = updatedProduct.Name;
    product.Price = updatedProduct.Price;
    product.Stocks = updatedProduct.Stocks;

    await _context.SaveChangesAsync();

    return product;
}

An explicit call to:

_context.Products.Update(product);

is not required here.

The entity returned by GetByIdAsync() is tracked by the current DbContext.

After its properties are changed, SaveChangesAsync() detects those modifications and generates the required UPDATE SQL.

Delete Product

public async Task<Product> DeleteByIdAsync(Guid id)
{
    Product product = await GetByIdAsync(id);

    _context.Products.Remove(product);

    await _context.SaveChangesAsync();

    return product;
}

Remove() marks the entity as deleted.

SaveChangesAsync() sends the required DELETE operation to the database.

Change Tracking

EF Core tracks entities loaded through normal queries.

Example:

Query Product
    ↓
DbContext tracks Product
    ↓
Change Name / Price / Stocks
    ↓
SaveChangesAsync()
    ↓
EF Core detects modifications
    ↓
UPDATE SQL is generated

For read-only queries:

.AsNoTracking()

can reduce unnecessary tracking overhead.

Important lesson from Day 08:

Tracked query
→ useful when an entity may be modified

AsNoTracking
→ useful for read-only retrieval

Async Database Operations

EF Core provides asynchronous methods such as:

ToListAsync()
FirstOrDefaultAsync()
SaveChangesAsync()

Database operations are mainly I/O-bound.

While waiting for the database, async/await allows ASP.NET Core to avoid blocking a request thread unnecessarily.

Send database request
    ↓
await
    ↓
thread can be used elsewhere
    ↓
database operation completes
    ↓
request continues

Controller and Service Responsibilities

The controller depends on:

IProductService

instead of directly depending on:

AppDbContext

This reduces tight coupling.

Controller responsibilities:

HTTP routes
Request DTOs
Response DTOs
Status codes
Logging

Service responsibilities:

Application operations
EF Core queries
Database changes
Persistence

Current dependency flow:

ProductController
    ↓
IProductService
    ↓
ProductService
    ↓
AppDbContext

Important PUT Bug Fixed

The PUT endpoint originally created a temporary Product object from the incoming request and then built the response from that same temporary object.

That could return the wrong ID.

Correct flow:

PUT request
    ↓
temporary Product contains incoming values
    ↓
UpdateByIdAsync(id, temporaryProduct)
    ↓
service loads existing database Product
    ↓
existing Product keeps original ID
    ↓
service updates properties
    ↓
SaveChangesAsync()
    ↓
service returns existing updated Product
    ↓
controller builds response from returned Product

The controller should therefore use the value returned by the service:

Product updatedProduct =
    await _productService.UpdateByIdAsync(id, product);

and build the response DTO from updatedProduct.

Persistence Difference

Earlier:

List<Product>
→ stored in RAM
→ application stops
→ data disappears

Now:

SQLite
→ stored in day08.db
→ application stops
→ data remains

This is the main persistence improvement introduced on Day 08.

Final Testing Checklist

Test the following using Swagger:

GET /api/Product

POST /api/Product

GET /api/Product/{id}

PUT /api/Product/{id}

DELETE /api/Product/{id}

Request a missing valid GUID and confirm the global exception handler returns 404

Stop and restart the application

Call GET /api/Product again and confirm stored products still exist

Confirm a PUT request preserves the original product ID

Confirm updated values are still present when the product is fetched again

For REST correctness, the POST endpoint should preferably return 201 Created rather than 200 OK.

Active Recall Questions and Answers

1. What is Entity Framework Core?

Entity Framework Core is a .NET ORM.

It maps C# entities to relational database structures and lets the application work with data using C# and LINQ instead of manually writing most SQL.

EF Core can translate supported LINQ queries into SQL and map database results back into C# objects.

2. What problem does an ORM solve?

An ORM reduces the amount of manual mapping required between object-oriented application code and relational database tables.

C# entities / LINQ
    ↓
ORM
    ↓
SQL / relational database

3. What is DbContext?

DbContext is the main bridge between the application and the database.

It is responsible for tasks such as:

Querying entities
Tracking entity changes
Adding entities
Removing entities
Saving changes

4. What is DbSet<Product>?

DbSet<Product> represents the set of Product entities managed by EF Core.

It acts as the application's EF Core access point to the Products table-like collection.

The Product class defines the structure of one product.

DbSet<Product> represents the collection of product entities.

5. What is an EF Core migration?

A migration is a versioned description of a database schema change.

It tracks how the database schema should change to match the application's current EF Core model.

6. What is the difference between migrations add and database update?

dotnet ef migrations add InitialCreate

generates migration files describing required schema changes.

dotnet ef database update

applies pending migrations to the real database.

7. Why did ProductService change from Singleton to Scoped?

The old singleton service owned an in-memory List<Product>, so one service instance was required to keep that list alive across HTTP requests.

After introducing EF Core, persistent data lives in the database.

AppDbContext is also scoped by default, so ProductService should be scoped when it depends on AppDbContext.

A singleton service should not depend on a scoped DbContext.

8. What does AsNoTracking() do?

AsNoTracking() tells EF Core not to track the entities returned by a query.

It is useful for read-only operations.

Tracking consumes additional memory and processing because the DbContext keeps information about entity states and changes.

9. Why should GetAllAsync use AsNoTracking()?

The products returned by GetAllAsync() are only being read and converted into response DTOs.

No modifications are expected, so change tracking is unnecessary.

10. Why should the current GetByIdAsync remain tracked?

The product returned from GetByIdAsync() may later be modified by UpdateByIdAsync() or marked for deletion by DeleteByIdAsync().

Tracking allows SaveChangesAsync() to detect those changes.

11. What does _context.Products.Add(product) do?

It tells EF Core to begin tracking the product in the Added state.

It does not normally mean that the row has already been inserted into the database.

12. What does SaveChangesAsync() do?

SaveChangesAsync() examines tracked entity changes, generates the required SQL operations, sends them to the database, and waits asynchronously for the database work to complete.

Examples include:

INSERT
UPDATE
DELETE

13. Why can UpdateByIdAsync work without calling _context.Products.Update(product)?

The product was loaded through the current DbContext, so EF Core is already tracking it.

When its properties change, EF Core detects those changes.

SaveChangesAsync() then generates the database UPDATE.

14. Why does SQLite data survive an application restart?

The old List<Product> was stored only in application memory.

When the process stopped, that memory disappeared.

SQLite stores the data in a database file on disk, so the data remains after the ASP.NET Core application stops.

15. Why use async EF Core methods in a Web API?

Database operations are I/O-bound.

Using async database methods avoids unnecessarily blocking request threads while the application waits for the database.

This helps ASP.NET Core handle more requests efficiently.

16. What is __EFMigrationsHistory?

It is a special table used by EF Core to record which migrations have already been applied.

Before applying migrations, EF Core checks this table to determine which migrations still need to run.

17. Why does EF Core recognize Id as a primary key?

EF Core uses conventions.

Properties named:

Id
<EntityName>Id

are normally recognized automatically as primary keys.

For a Product entity, both Id and ProductId follow this convention.

18. What does AddDbContext<AppDbContext>() do?

It registers AppDbContext with ASP.NET Core dependency injection.

This allows services such as ProductService to receive AppDbContext through constructor injection.

19. What does UseSqlite() do?

It configures EF Core to use the SQLite database provider.

20. What does GetConnectionString("DefaultConnection") do?

It reads the named database connection string from application configuration, normally appsettings.json.

21. Why did the original PUT response return the wrong ID?

The controller created a temporary Product from the PUT body and used that temporary object when building the response.

The correct existing entity was returned by UpdateByIdAsync().

The controller needed to capture and use that returned product so the response preserved the original database ID.

22. Why should ProductController depend on IProductService?

Depending on the interface reduces tight coupling between the controller and the concrete service/database implementation.

The controller does not need to know whether the service uses SQLite, PostgreSQL, another database, or a different implementation.

23. What is the difference between DbSet<Product> and ProductService?

DbSet<Product> is EF Core's representation of the Product entity set and provides database-oriented operations for Product entities.

ProductService uses that entity set to implement application operations such as retrieving, adding, updating, and deleting products.

ProductService
    ↓
DbSet<Product>
    ↓
Database

Interview Review Notes

The main Day 08 concepts were understood correctly.

Areas that required clarification during review:

EF Core vs ASP.NET Core responsibilities
Schema migrations vs moving data
DbContext tracking vs the database storing a duplicate
I/O-bound work vs CPU-bound work
Tracked entities and SaveChangesAsync()
Temporary PUT object vs existing database entity
Scoped DbContext lifetime

These are useful active-recall topics to revisit before a .NET backend interview.

Day 08 Completion Summary

By the end of Day 08, the project moved from temporary in-memory data storage to a persistent EF Core + SQLite implementation.

Concepts practiced:

ORM
Entity Framework Core
DbContext
DbSet<T>
SQLite provider
Connection strings
Dependency injection
Scoped lifetime
EF Core migrations
Migration history
LINQ-to-SQL translation
Async database operations
Change tracking
AsNoTracking
Persistent CRUD
Controller/service separation

The next logical step is to build on this database foundation with more realistic relational modeling, query design, and production-style persistence patterns.