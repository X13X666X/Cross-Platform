
using System;

namespace ShopCatalog;

class Program
{
    static MyLinkedList<Shop> shops = new MyLinkedList<Shop>();

    static void Main()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n========== SHOP CATALOG ==========");
            Console.WriteLine("1. Add a shop");
            Console.WriteLine("2. Display all shops");
            Console.WriteLine("3. Delete a shop");
            Console.WriteLine("4. Sort shops");
            Console.WriteLine("5. Search for shops");
            Console.WriteLine("6. Clear catalog");
            Console.WriteLine("0. Exit");
            Console.WriteLine("==================================");

            Console.Write("Your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddShop();
                    break;

                case "2":
                    ShopUtilities.Print(shops);
                    break;

                case "3":
                    DeleteShop();
                    break;

                case "4":
                    SortShops();
                    break;

                case "5":
                    FindShops();
                    break;

                case "6":
                    shops.Clear();
                    Console.WriteLine("Catalog cleared.");
                    break;

                case "0":
                    running = false;
                    Console.WriteLine("Program terminated.");
                    break;

                default:
                    Console.WriteLine("Invalid menu option.");
                    break;
            }
        }
    }

    static void AddShop()
    {
        Console.WriteLine("\n--- Add a New Shop ---");

        Console.Write("Name: ");
        string name = Console.ReadLine();

        Console.Write("Address: ");
        string address = Console.ReadLine();

        Console.Write("Specialization: ");
        string specialization = Console.ReadLine();

        try
        {
            Shop shop = new Shop(name, address, specialization);

            Console.Write("\nNumber of phone numbers: ");
            int phoneCount = ReadNumber();

            for (int i = 0; i < phoneCount; i++)
            {
                Console.Write($"Phone #{i + 1}: ");
                shop.AddPhone(Console.ReadLine());
            }

            string[] days =
            {
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
                "Sunday"
            };

            Console.WriteLine("\nEnter the working schedule:");

            foreach (string day in days)
            {
                Console.Write($"{day} (e.g. 09:00-18:00 or Closed): ");
                string hours = Console.ReadLine();

                shop.AddWorkingHours(day, hours);
            }

            shops.Add(shop);

            Console.WriteLine("\nShop successfully added.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void DeleteShop()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("The catalog is empty.");
            return;
        }

        ShopUtilities.Print(shops);

        Console.Write("\nEnter the shop number to delete: ");
        int index = ReadNumber();

        if (index < 1 || index > shops.Count)
        {
            Console.WriteLine("Invalid shop number.");
            return;
        }

        shops.RemoveAt(index - 1);

        Console.WriteLine("Shop successfully deleted.");
    }

    static void SortShops()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("The catalog is empty.");
            return;
        }

        Console.WriteLine("\nSort by:");
        Console.WriteLine("1. Name");
        Console.WriteLine("2. Address");
        Console.WriteLine("3. Specialization");

        Console.Write("Your choice: ");
        string choice = Console.ReadLine();

        MyLinkedList<Shop> sortedShops;

        switch (choice)
        {
            case "1":
                sortedShops = ShopUtilities.SortBy(
                    shops,
                    shop => shop.Name,
                    StringComparer.CurrentCultureIgnoreCase);
                break;

            case "2":
                sortedShops = ShopUtilities.SortBy(
                    shops,
                    shop => shop.Address,
                    StringComparer.CurrentCultureIgnoreCase);
                break;

            case "3":
                sortedShops = ShopUtilities.SortBy(
                    shops,
                    shop => shop.Specialization,
                    StringComparer.CurrentCultureIgnoreCase);
                break;

            default:
                Console.WriteLine("Invalid menu option.");
                return;
        }

        Console.WriteLine("\n--- Sorted Shop Catalog ---");
        ShopUtilities.Print(sortedShops);
    }

    static void FindShops()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("The catalog is empty.");
            return;
        }

        Console.WriteLine("\nSearch by:");
        Console.WriteLine("1. Name");
        Console.WriteLine("2. Specialization");

        Console.Write("Your choice: ");
        string choice = Console.ReadLine();

        Console.Write("Enter search query: ");
        string query = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(query))
        {
            Console.WriteLine("Search query cannot be empty.");
            return;
        }

        MyLinkedList<Shop> result;

        switch (choice)
        {
            case "1":
                result = ShopUtilities.Find(
                    shops,
                    shop => shop.Name.Contains(
                        query,
                        StringComparison.OrdinalIgnoreCase));
                break;

            case "2":
                result = ShopUtilities.Find(
                    shops,
                    shop => shop.Specialization.Contains(
                        query,
                        StringComparison.OrdinalIgnoreCase));
                break;

            default:
                Console.WriteLine("Invalid menu option.");
                return;
        }

        Console.WriteLine("\n--- Search Results ---");
        ShopUtilities.Print(result);
    }

    static int ReadNumber()
    {
        while (true)
        {
            string input = Console.ReadLine();

            if (int.TryParse(input, out int number) && number >= 0)
            {
                return number;
            }

            Console.Write("Enter a non-negative integer: ");
        }
    }
}
