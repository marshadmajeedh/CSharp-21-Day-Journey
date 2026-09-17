using System.Collections.Generic;
using System.Linq;
using System;
Console.WriteLine("Generics : \n");
Box<string> boxString = new()
{
    Value = "12345"
};

Box<int> boxInteger = new()
{
    Value = 12345
};

Box<double> boxDouble = new()
{
    Value = 12345D
};

boxString.PrintValue();
boxDouble.PrintValue();
boxInteger.PrintValue();

Console.WriteLine("\nGenerics Storage class for string: \n");
Storage<string> names = new();

names.AddItem("Marshad");
names.AddItem("Ahmed");
names.AddItem("Nimal");

names.PrintAll();

Console.WriteLine(names.TotalItemsInTheList());

Console.WriteLine("\nGenerics Storage class for integer: \n");
Storage<int> ages = new();

ages.AddItem(10);
ages.AddItem(20);
ages.AddItem(30);

ages.PrintAll();

Console.WriteLine(ages.TotalItemsInTheList());

Console.WriteLine("\nGenerics USer Storage class for Users: \n");
Customer customer1 = new()
{
    Name = "Marshad",
    Email = "marshadahamedh@gmail.com",
    LoyaltyPoints = 100
};

Customer customer2 = new()
{
    Name = "arshad",
    Email = "arshadahamedh@gmail.com",
    LoyaltyPoints = 200
};

Customer customer3 = new ()
{
    Name = "shad",
    Email = "shadahamedh@gmail.com",
    LoyaltyPoints = 300
};
UserStorage<Customer> customers = new();
customers.Add(customer1);
customers.Add(customer2);
customers.Add(customer3);

customers.PrintNames();

//Console.WriteLine("\nGenerics USer Storage class for wrong type: \n");
//UserStorage<int> numbers = new();
Console.WriteLine();
//Task 01
List<string> productNames = [];
productNames.Add("Mouse");
productNames.Add("Headphones");
productNames.Add("Keyboard");

foreach(string s in productNames)
{
    Console.WriteLine(
        $"Product name is: {s}"
    );
}

//Task 02
Dictionary<long, string> productsWithIdAndName = [];
long id = 1;
foreach (string s in productNames)
{
    productsWithIdAndName.Add(id++, s);
}

string productWithId2 = "";
if (productsWithIdAndName.TryGetValue(2, out string? productName2))
{
    productWithId2 = productName2;
}
else
{
    productWithId2 = "Not found";
}
Console.WriteLine(
    $"\nProduct name with id equals 2 is: {productWithId2}\n"
);

string productWithId999 = "";
if (productsWithIdAndName.TryGetValue(999, out string? productName999))
{
    productWithId999 = productName999;
}
else
{
    productWithId999 = "Not found";
}
Console.WriteLine(
    $"\nProduct name with id equals 999 is: {productWithId999}\n"
);

//Task 03
HashSet<string> emails = [];
emails.Add("marshad@gmail.com");
emails.Add("marshad@gmail.com");
emails.Add("arshad@gmail.com");

foreach (var mail in emails)
{
    Console.WriteLine(
        $"Your e-mail is: {mail}"
    );
}

if (!emails.Add("arshad@gmail.com"))
{
    Console.WriteLine(
        "\nThis e-mail is already exist in the system please try adding different email"
    );
}

Console.WriteLine("\nLambda Expressions Func<>: ");
Func<int, bool> isEven = number => number % 2 == 0;
int input = 2;
Console.WriteLine(
    $"Is {input} Even number: {isEven(input)}\n"
);

Func<decimal, decimal, decimal> add = (a, b) => a + b;
decimal input1 = 22.3m;
decimal input2 = 44.2m;
Console.WriteLine(
    $"Addition of {input1} and {input2} is: {add(input1, input2)}\n"
);

Func<decimal, int, decimal> calculateTotal = (a, b) => a * b;
decimal inputPrice = 22.3m;
int inputQuantity = 44;
Console.WriteLine(
    $"Total calculations of price {inputPrice} and quantity {inputQuantity} is: {calculateTotal(inputPrice, inputQuantity)}\n"
);

Console.WriteLine("\nLambda Expressions Action<>: ");
Action<string> printMessage = text => Console.WriteLine("Your message is : " + text);
Func<string,bool> productNameLengthGreaterThanFive = text => text.Length > 5;

foreach (var v in productNames)
{
    printMessage(v);
    Console.WriteLine("Has more than 5 characters: "+(productNameLengthGreaterThanFive(v) ? "Yes\n" : "No\n"));
}
Console.WriteLine();
Console.WriteLine("\nLINQ expressions WHERE(): ");
var productNamesFiveCharactersLong = productNames.Where(productNameLengthGreaterThanFive);

//Task of LINQ expressions

//Using
List<int> numbers =
[
    5, 12, 3, 20, 8, 15, 1
];

/**
produce the result:
40
30
24

But follow these rules:
Keep only numbers greater than 10
→ 12, 20, 15

Sort descending
→ 20, 15, 12

Multiply each by 2
→ 40, 30, 24
**/

Func<int, bool> numbersGreaterThanTen = num => num > 10;
Func<int, int> multiplyBy2 = num => num * 2;

var numGreaterThanTen = numbers.Where(numbersGreaterThanTen);

Console.WriteLine("Numbers greater than 10 : ");
foreach (var num in numGreaterThanTen)
{
    Console.WriteLine(
        $"number : {num}"
    );
}

Console.WriteLine();
var sortDescending = numbers.OrderByDescending(num => num);
Console.WriteLine("Numbers order by descending : ");
foreach (var num in sortDescending)
{
    Console.WriteLine(
        $"number : {num}"
    );
}

Console.WriteLine();
var multiplyEachByTwo = numbers.Select(multiplyBy2);
Console.WriteLine("Numbers multiplied by 2 : ");
foreach (var num in multiplyEachByTwo)
{
    Console.WriteLine(
        $"number : {num}"
    );
}

Console.WriteLine();
var results = numbers
    .Where(num => num > 10)
    .OrderByDescending(num => num)
    .Select(num => num * 2);
Console.WriteLine("Result of chained method LINQ : ");
foreach (var result in results)
{
    Console.WriteLine(
        $"result : {result}"
    );
}

Console.WriteLine();
bool greaterThan100 = numbers.Any(num => num > 100);
Console.WriteLine("Number greater than 100 exist ? "+(greaterThan100 ? "Yes":"No"));
Console.WriteLine();

Console.WriteLine();
bool greaterThanZero = numbers.All(num => num > 0);
Console.WriteLine("Every number is greater than 0 ? "+(greaterThanZero ? "Yes":"No"));
Console.WriteLine();


//Second exercise of LINQ
List<int> values =
[
    10,
    20,
    20,
    30
];

Console.WriteLine("Using First: " + values.First(val => val >= 20));
Console.WriteLine("Using FirstOrDefault: " + values.FirstOrDefault(val => val > 100));
Console.WriteLine("Using Single: " + values.Single(val => val >= 30));
//Exceptions
try
{
    Console.WriteLine("Using Single: " + values.Single(val => val >= 20));
}
catch (InvalidOperationException ioe)
{
    Console.WriteLine(ioe.Message);
}
Console.WriteLine("Using SingleOrDefault: " + values.SingleOrDefault(val => val >= 100));


//aggregation methods
List<int> scores =
[
    45,
    67,
    82,
    90,
    56,
    90
];

Console.WriteLine(
    $"\nSummation of this list is : {scores.Sum()}\n"
);

Console.WriteLine(
    $"Number of elements in this list is: {scores.Count}\n"
);

Console.WriteLine(
    $"Average of this list is : {scores.Average()}\n"
);

Console.WriteLine(
    $"Minimum number in this list is: {scores.Min()}\n"
);

Console.WriteLine(
    $"Maximum number in this list is: {scores.Max()}\n"
);

//GroupBy()
List<string> languages =
[
    "C#",
    "Java",
    "C#",
    "Python",
    "Java",
    "C#"
];

var groupBySameLanguages = languages.GroupBy(lang => lang);
foreach (var group in groupBySameLanguages)
{
    Console.WriteLine(
        $"Group key: {group.Key}, Group count: {group.Count()}"
    );
}
Console.WriteLine();

var numberOfScoresGreaterThan50 = scores
    .Count(score => score >= 50);

Console.WriteLine("\nDay 3 real exercise");
List<Product> products =
[
    new()
    {
        Id = 1,
        Name = "Mechanical Keyboard",
        Category = "Accessories",
        Price = 12000m,
        Stock = 10
    },

    new()
    {
        Id = 2,
        Name = "Gaming Mouse",
        Category = "Accessories",
        Price = 7000m,
        Stock = 20
    },

    new()
    {
        Id = 3,
        Name = "27 Inch Monitor",
        Category = "Displays",
        Price = 65000m,
        Stock = 5
    },

    new()
    {
        Id = 4,
        Name = "24 Inch Monitor",
        Category = "Displays",
        Price = 45000m,
        Stock = 8
    },

    new()
    {
        Id = 5,
        Name = "USB Cable",
        Category = "Accessories",
        Price = 1500m,
        Stock = 0
    }
];
//Task 1
var productWithStockGreaterThanZero = products
    .Where(product => product.Stock > 0)
    .OrderByDescending(product => product.Price);
foreach (var pwsgz in productWithStockGreaterThanZero)
{
    Console.WriteLine(
        $"Product name : {pwsgz.Name}"
    );
}

Console.WriteLine();
//Task 2
var selectProductByName = productWithStockGreaterThanZero
    .Select(product => product.Name);
foreach (var spbn in selectProductByName)
{
    Console.WriteLine(
        $"Product name : {spbn}"
    );
}

Console.WriteLine();
//Task 3
var totalInventoryValue = products
    .Sum(product => product.Stock * product.Price);
Console.WriteLine(
        $"Total inventory value : {totalInventoryValue}"
);

Console.WriteLine();
//Task 4
bool anyProductOutOfStock = products
    .Any(product => product.Stock == 0);
Console.WriteLine(
        $"Any product has out of stock : {(anyProductOutOfStock ? "Yes" : "No")}"
);

Console.WriteLine();
//Task 5
var productWithId3 = products
    .FirstOrDefault(product => product.Id == 3);
if (productWithId3 != null)
{
    Console.WriteLine(
        $"Product name with id 3 is : {productWithId3.Name}"
    );
} else
{
    Console.WriteLine(
        "Product name with id 3 is Not found "
    );
}

Console.WriteLine();
//Task 6
var groupByCategory = products
    .GroupBy(product => product.Category);
foreach(var group in groupByCategory)
{
    Console.WriteLine(
        $"Product key: {group.Key}, Product count: {group.Count()}"
    );
}

Console.WriteLine();
//Task 7
//For each category, calculate total inventory value.

foreach (var group in groupByCategory)
{
    decimal totalValue = group.Sum(product => product.Stock * product.Price);
    Console.WriteLine(
        $"Category type {group.Key}, total value is {totalValue}"
    );
}

