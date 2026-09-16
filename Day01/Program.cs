Product product1 = new("Mechanical Keyboard",1000m,0);
Product product2 = new("Wood Keyboard",2000m,4);
Product product3 = new("Plastic Keyboard", 3000m, 200);

Console.WriteLine("Try-Catch blocks for price");
try
{
    Product product4 = new("Aluminum keyboard", -200m, 20);
}
catch (ArgumentException e)
{
    Console.WriteLine(e.Message);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
Console.WriteLine();
Console.WriteLine("Try-Catch blocks for Name");
try
{
    Product product5 = new("", 500m, 30);
}
catch (ArgumentException e)
{
    Console.WriteLine(e.Message);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
Console.WriteLine();
Console.WriteLine("Try-Catch blocks for stocks");
try
{
    Product product5 = new("Jennifer", 500m, -30);
}
catch (ArgumentException e)
{
    Console.WriteLine(e.Message);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}



List<Product> products = [];
products.Add(product1);
products.Add(product2);
products.Add(product3);

//Task 1
foreach (Product product in products)
{
    Console.WriteLine(
        $"Product Id: {product.Id}\n"+
        $"Product Name: {product.Name}\n"+
        $"Product Price: {product.Price}\n"+
        $"Available Stocks: {product.Stocks}\n"
    );
}

//Task 2
var productStockGreaterThanNine = (
    products.Where(p => p.Stocks >= 10)
);

foreach (Product product in productStockGreaterThanNine)
{
    Console.WriteLine(
        $"Product Id: {product.Id}\n"+
        $"Product Name: {product.Name}\n"+
        $"Product Price: {product.Price}\n"+
        $"Available Stocks: {product.Stocks}\n"
    );
}


//Task 3
var totalInventoryValue = (
    products.Sum(p => p.Stocks * p.Price)
);

Console.WriteLine(
    $"Total Inventory Value: {totalInventoryValue}\n"
);

//Task 4
var productWithIdEqualsTwo =(
    products.FirstOrDefault(p => p.Id == 2)
);

string productName = productWithIdEqualsTwo?.Name ?? "Product with id 2 does not exist in the list\n";

Console.WriteLine(
    $"Product with id equals 2 is: {productName}\n"
);


var searchingForValueThatDoesNotExist = (
    products.FirstOrDefault(p => p.Id == 999)
);

string name = searchingForValueThatDoesNotExist?.Name ?? "Not found\n";
Console.WriteLine(
    $"Does a product with id exist in the list: {name}"
);

string? description = null;
description ??= "Description is not available\n";

Console.WriteLine(description);

Console.WriteLine(
    "Lets print status of the each products"
);

foreach (Product product in products)
{
    Console.WriteLine(
        $"Product Id: {product.Id}\n"+
        $"Product status: {product.Status()}\n"
    );
}