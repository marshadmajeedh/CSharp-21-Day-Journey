ProductDto productDto = new(1, "Wood Keyboard", 1000m);
Console.WriteLine(productDto);

ProductDto updatedProductDTO = productDto with
{
    Price = 2000m
};
Console.WriteLine(updatedProductDTO);

Console.WriteLine("\nStudent DTO : ");
StudentDto student1 = new(1, "Marshad", 3.5);
StudentDto student2 = new(1, "Marshad", 3.5);
Console.WriteLine("Student 1 : ");
Console.WriteLine(student1);
Console.WriteLine($"\nValue based comparison rather than instance, student1 == student2? : {student1 == student2}");

//student1.Name = "Ahamed";

StudentDto updatedStudent = student1 with
{
    Name = "Fathima",
    GPA = 3.8
};
Console.WriteLine("Student 1 : " + student1);
Console.WriteLine("Updated student : " + updatedStudent);

Console.WriteLine("\nPatterns in c# : ");

Employee employee = new()
{
    Name = "Marshad",
    Age = 24,
    Salary = 85000m
};

//Task 1
if (employee is { Salary: > 50000m })
{
    Console.WriteLine("Has good position in the company");
}

if(employee is {Age: >= 18 })
{
    Console.WriteLine("Adult employee\n");
}

//Task 02
var salaryStatus = employee.Salary switch
{
    >= 100000m => "High salary",
    >= 50000m => "Medium salary",
    _ => "Low salary"
};
Console.WriteLine(salaryStatus+"\n");

//Task 03
var ageStatus = employee.Age switch
{
    >= 18 and <= 25 => "Young adult",
    > 25 and <= 40 => "Adult",
    > 40 => "Senior",
    _ => "Minor"
};

Console.WriteLine(ageStatus + "\n");

//Tuple and de-construction
List<int> numbers =
[
    12,
    5,
    99,
    23,
    1
];

var (minimum, maximum) = Util.GetMinMax(numbers);
Console.WriteLine("Minimum number is : " + minimum);
Console.WriteLine("Maximum number is : " + maximum + "\n");

UserProfile user = new()
{
    Username = "Marshad",
    Bio = "Software engineering undergraduate"
};

Console.WriteLine(
    $"Your profile bio is: {user.Bio?.ToUpper() ?? "Bio is un-available"}"
);

Util.PrintBio(user.Bio);

try
{
    int length1 = Util.GetBioLength(user.Bio);

    Console.WriteLine(
        $"Length of {user.Bio} is : {length1}"
    );

}
catch (ArgumentNullException ane)
{
    Console.WriteLine(
        $"Error : {ane.Message}"
    );
}

try
{
    int length2 = Util.GetBioLength(null);
}
catch (ArgumentNullException ane)
{
    Console.WriteLine(
        $"Error : {ane.Message}"
    );
}

Console.WriteLine("\ninit and required section : \n");
Account account = new()
{
    Username = "Marshad",
    Email = "marshadahamedh@gmail.com"
};

Console.WriteLine($"Username : {account.Username}");
Console.WriteLine($"Email : {account.Email}");
Console.WriteLine($"Created at : {account.CreatedAt}\n");

/** Account badAccountType = new();

error CS9035: Required member 'Account.Username' must be set in the object initializer or attribute constructor.
Required member 'Account.Email' must be set in the object initializer or attribute constructor.
The build failed. Fix the build errors and run again.

**/

/**
account.Username = "Unknown";

error CS8852: Init-only property or indexer 'Account.Username' can only be assigned in an object initializer, or on 'this' or
'base' in an instance constructor or an 'init' accessor.

The build failed. Fix the build errors and run again.

**/

Console.WriteLine("Day 4 final exercise: immutable account design\n");

AccountProfile accountProfile = new()
{
    Username = "Marshad",
    Email = "marshadahamed@gmail.com"
};

Console.WriteLine($"Profile id: {accountProfile.Id}");
Console.WriteLine($"Profile username: {accountProfile.Username}");
Console.WriteLine($"Profile e-mail: {accountProfile.Email}");
Console.WriteLine($"Profile was created in: {accountProfile.CreatedAt}");
Console.WriteLine($"Profile was lastly logged in: {accountProfile.LastLoginAt}\n");
accountProfile.MarkLogin();
Console.WriteLine(
    $"Last logged in: {accountProfile.LastLoginAt}\n"
);

Console.WriteLine("-Try breaking things commented out- \n");
//Console.WriteLine($"Profile username: {accountProfile.Username = "Ahmed"}");
//Console.WriteLine($"Last logged in: {accountProfile.LastLoginAt = DateTime.Now}");
//accountProfile.LastLoginAt = "Whatever";

AccountProfileDto apd = new
(
    accountProfile.Id,
    accountProfile.Username,
    accountProfile.Email,
    accountProfile.CreatedAt,
    accountProfile.LastLoginAt
);

Console.WriteLine(apd);