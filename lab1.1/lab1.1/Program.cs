
using System;
using System.Collections.Generic;
using System.Linq;

class Toy
{
    public string Name { get; set; }
    public int Price { get; set; } // Вартість у копійках
    public int MinAge { get; set; }
    public int MaxAge { get; set; }

    public Toy(string name, int price, int minAge, int maxAge)
    {
        Name = name;
        Price = price;
        MinAge = minAge;
        MaxAge = maxAge;
    }
}

class Cubes : Toy
{
    public int Quantity { get; set; }

    public Cubes(int price, int minAge, int maxAge, int quantity)
        : base("Кубики", price, minAge, maxAge)
    {
        Quantity = quantity;
    }
}

class Program
{
    static void Main()
    {
        List<Cubes> toys = new List<Cubes>
        {
            new Cubes(3500, 1, 5, 20),
            new Cubes(2500, 2, 6, 30),
            new Cubes(1500, 1, 4, 15),
            new Cubes(5000, 2, 7, 50),
            new Cubes(4500, 3, 8, 40),
            new Cubes(2000, 2, 5, 25),
            new Cubes(6000, 1, 3, 60)
        };

        Console.Write("Введіть максимальну вартість іграшки (грн): ");
        decimal maxPrice = decimal.Parse(Console.ReadLine());

        var result = toys
            .Where(t => t.MinAge <= 2 && t.MaxAge >= 2)
            .Where(t => t.Price >= 2000 && t.Price <= maxPrice * 100)
            .OrderByDescending(t => t.Price);

        Console.WriteLine("\nКубики, які підходять дитині 2 років:");
        Console.WriteLine("--------------------------------------");

        foreach (var toy in result)
        {
            Console.WriteLine(
                $"Назва: {toy.Name}, " +
                $"Ціна: {toy.Price / 100.0:F2} грн, " +
                $"Вік: від {toy.MinAge} до {toy.MaxAge} років, " +
                $"Кількість: {toy.Quantity} шт.");
        }
    }
}
