using static System.Console;
using System.Collections.Generic;
Staff staff1 = new()
{
    Id = 2,
    Email = "marshad@example.com",
    Name = "Marshad",
    Department = "Software Engineering"
};

User user1 = new()
{
    Id = 1,
    Email = "user@example.com",
    Name = "User",
};

Customer customer1 = new()
{
    Id = 3,
    Email = "customer@example.com",
    Name = "Customer",
    LoyaltyPoints = 100
};

List<User> users =
[
    staff1,
    user1,
    customer1
];



staff1.PrintUser();
Console.WriteLine();
user1.PrintUser();
Console.WriteLine();
customer1.PrintUser();
Console.WriteLine();

foreach(User user in users)
{
    Console.WriteLine(user.Describe());
}


Console.WriteLine("Interface & Abstraction section");
IPaymentProcessor payment1 = new  CashPaymentProcessor();
IPaymentProcessor payment2 = new CardPaymentProcessor();
payment1.ProcessPayment(500m);
payment2.ProcessPayment(600m);

Console.WriteLine("Dependency Injection section");
OrderService orderService1 = new(new CardPaymentProcessor());
OrderService orderService2 = new(new CashPaymentProcessor());

orderService1.CheckOut(400m);
orderService2.CheckOut(800m);

Console.WriteLine("Abstract class\n");

Notification smsNotification = new SMSNotification
{
    Recipient = "0766756648"
};

Notification emailNotification = new EmailNotification
{
    Recipient = "marshadahamedh@gmail.com"
};

smsNotification.PrintRecipient();
smsNotification.Send();

emailNotification.PrintRecipient();
emailNotification.Send();

Console.WriteLine("Composition \n");
Car petrolCar = new(new PetrolEngine());
Car electricCar = new(new ElectricMotor());
petrolCar.StartCar();
petrolCar.StopCar();

electricCar.StartCar();
electricCar.StopCar();