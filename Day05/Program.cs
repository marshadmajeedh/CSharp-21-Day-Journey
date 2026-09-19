using System.Diagnostics;
Console.WriteLine("Program started");

await Task.Delay(3000);

Console.WriteLine("Operation completed");

Console.WriteLine("Program finished");

Task operation = Task.Delay(3000);
await operation;
Console.WriteLine("Program finished\n");

Console.WriteLine("(1) Task");
Console.WriteLine("Program started...");
await Util.SimulateDownloadAsync();
Console.WriteLine("Program finished.\n");

Console.WriteLine("(2) Task<T>");
Console.WriteLine("Program started...");
string username = await Util.GetUsernameAsync();
Console.WriteLine(username);
Console.WriteLine("Program finished.\n");

//exercise 1
Console.WriteLine("(3) Task");
await Util.PrintMessageAsync();

//exercise 2
Console.WriteLine("(4) Task<int>");
Console.WriteLine("Program started...");
var value  = await Util.CalculateAsync(10);
Console.WriteLine($"Result: {value}");
Console.WriteLine("Program finished.\n");

//exercise 3
Console.WriteLine("(5) Task<T>");
Console.WriteLine("Program started...");
string productName = await Util.GetProductNameAsync(1);
Console.WriteLine($"Product name is: {productName}");
string missingProductName = await Util.GetProductNameAsync(999);
Console.WriteLine($"Product name is: {missingProductName}");
Console.WriteLine("Program finished.\n");

//exercise 4
Console.WriteLine("(6) Task.whenAll");

Console.WriteLine("This should take around 6 seconds total (2 + 2 + 2)");
Stopwatch stopwatch1 = Stopwatch.StartNew();
var order1 = await Util.GetOrderAsync(1);
var order2 =await  Util.GetOrderAsync(2);
var order3 = await Util.GetOrderAsync(3);
stopwatch1.Stop();

Console.WriteLine(
    $"Elapsed: {stopwatch1.ElapsedMilliseconds} ms"
);
Console.WriteLine(
    $"Order 1: {order1}\n" +
    $"Order 2: {order2}\n" +
    $"Order 3: {order3}\n"
);

Console.WriteLine("We can fix it by using Task.WhenAll if those async works are independent");
Stopwatch stopwatch2 = Stopwatch.StartNew();

Task<string> order4 = Util.GetOrderAsync(4);
Task<string> order5 = Util.GetOrderAsync(5);
Task<string> order6 = Util.GetOrderAsync(6);
string[] results =
    await Task.WhenAll(
        order4,
        order5,
        order6
    );
stopwatch2.Stop();

Console.WriteLine(
    $"Elapsed: {stopwatch2.ElapsedMilliseconds} ms"
);

foreach (var result in results)
{
    Console.WriteLine(
        $"{result}"
    );
}

//exercise 5
Console.WriteLine("\n(7) async exception");

try
{
    Task<string> order7 = Util.GetOrderAsync(-1);

    string task = await order7;

    Console.WriteLine(
        $"{task}"
    );
}
catch (ArgumentException e)
{
    Console.WriteLine(
        $"Error: {e.Message}"
    );
}

//exercise 6
Console.WriteLine("\n(8) async exception");

//success ful call 
try
{
    decimal amount = await Util.ProcessPaymentAsync(5000m);

    Console.WriteLine(
        $"Payment successful: {amount}"
    );
}
catch (ArgumentException ex)
{
    Console.WriteLine(
        $"Payment failed: {ex.Message}"
    );
}

//un-success ful call 
try
{
    decimal amount = await Util.ProcessPaymentAsync(-1m);

    Console.WriteLine(
        $"Payment successful: {amount}"
    );
}
catch (ArgumentException ex)
{
    Console.WriteLine(
        $"Payment failed: {ex.Message}"
    );
}

Console.WriteLine("\n(9) Concurrent async payments");
//Trying them cun-currently
Task<decimal> amount1 = Util.ProcessPaymentAsync(500m);
Task<decimal> amount2 = Util.ProcessPaymentAsync(1000m);

try
{
    decimal[] paymentResults = await Task.WhenAll
    (
        amount1,
        amount2
    );

    foreach (var pr in paymentResults)
    {
        Console.WriteLine(
        $"Payment successful: {pr}"
    );
    }
}
catch (ArgumentException e)
{
    Console.WriteLine(
        $"Payment failed: {e.Message}"
    );
}

Console.WriteLine("\n(10) async vs sync");

decimal total = Util.CalculateOrderTotal(25000m, 5);
string result1 = await Util.SaveOrderAsync(2, total);
Console.WriteLine(
    $"Sync result: {total}"
);
Console.WriteLine(
    $"Async result: {result1}\n"
);
