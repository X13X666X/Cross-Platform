using System;
using System.IO;
using System.Text;

namespace ShopCatalog14;

class Program
{
    static MyLinkedList<Shop> shops = new MyLinkedList<Shop>();

    static void Main(string[] args)
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        // Check command-line argument
        if (args.Length > 0 &&
            args[0].Equals("-auto", StringComparison.OrdinalIgnoreCase))
        {
            RunAutomaticMode();
        }
        else
        {
            RunDialogMode();
        }
    }

    // ==========================================
    // DIALOG MODE
    // ==========================================

    static void RunDialogMode()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n========== SHOP CATALOG ==========");
            Console.WriteLine("1. Add a shop");
            Console.WriteLine("2. Display all shops");
            Console.WriteLine("3. Edit a shop");
            Console.WriteLine("4. Delete a shop");
            Console.WriteLine("5. Sort shops");
            Console.WriteLine("6. Search shops");
            Console.WriteLine("7. Save catalog");
            Console.WriteLine("8. Load catalog");
            Console.WriteLine("9. Clear catalog");
            Console.WriteLine("0. Exit");
            Console.WriteLine("==================================");

            Console.Write("Your choice: ");
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    AddShop();
                    break;

                case "2":
                    ShopUtilities.Print(shops);
                    break;

                case "3":
                    EditShop();
                    break;

                case "4":
                    DeleteShop();
                    break;

                case "5":
                    SortShops();
                    break;

                case "6":
                    FindShops();
                    break;

                case "7":
                    SaveCatalog();
                    break;

                case "8":
                    LoadCatalog();
                    break;

                case "9":
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

    // ==========================================
    // ADD SHOP
    // ==========================================

    static void AddShop()
    {
        Console.WriteLine("\n--- Add a New Shop ---");

        try
        {
            Console.Write("Name: ");
            string name = ReadText();

            Console.Write("Address: ");
            string address = ReadText();

            Console.Write("Specialization: ");
            string specialization = ReadText();

            Shop shop = new Shop(name, address, specialization);

            Console.Write("Number of phones: ");
            int phoneCount = ReadNumber();

            for (int i = 0; i < phoneCount; i++)
            {
                Console.Write($"Phone #{i + 1}: ");
                shop.AddPhone(ReadText());
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

            Console.WriteLine("\nEnter working hours:");

            foreach (string day in days)
            {
                Console.Write($"{day} (09:00-18:00 or Closed): ");
                shop.SetSchedule(day, ReadText());
            }

            shops.Add(shop);

            Console.WriteLine("Shop successfully added.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // ==========================================
    // EDIT SHOP
    // ==========================================

    static void EditShop()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("Catalog is empty.");
            return;
        }

        ShopUtilities.Print(shops);

        Console.Write("\nEnter shop number: ");
        int index = ReadNumber();

        if (index < 1 || index > shops.Count)
        {
            Console.WriteLine("Invalid shop number.");
            return;
        }

        Shop shop = shops.Get(index - 1);

        Console.WriteLine("\n1. Edit main information");
        Console.WriteLine("2. Add phone");
        Console.WriteLine("3. Remove phone");
        Console.WriteLine("4. Edit working hours");

        Console.Write("Your choice: ");
        string choice = Console.ReadLine() ?? "";

        try
        {
            switch (choice)
            {
                case "1":
                    Console.Write("New name: ");
                    string name = ReadText();

                    Console.Write("New address: ");
                    string address = ReadText();

                    Console.Write("New specialization: ");
                    string specialization = ReadText();

                    shop.Update(name, address, specialization);

                    Console.WriteLine("Shop information updated.");
                    break;

                case "2":
                    Console.Write("New phone: ");
                    shop.AddPhone(ReadText());

                    Console.WriteLine("Phone added.");
                    break;

                case "3":
                    if (shop.Phones.Length == 0)
                    {
                        Console.WriteLine("No phones available.");
                        break;
                    }

                    for (int i = 0; i < shop.Phones.Length; i++)
                        Console.WriteLine($"{i + 1}. {shop.Phones[i]}");

                    Console.Write("Phone number to remove: ");
                    int phoneIndex = ReadNumber();

                    if (phoneIndex < 1 || phoneIndex > shop.Phones.Length)
                    {
                        Console.WriteLine("Invalid phone number.");
                        break;
                    }

                    shop.RemovePhone(phoneIndex - 1);

                    Console.WriteLine("Phone removed.");
                    break;

                case "4":
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

                    for (int i = 0; i < days.Length; i++)
                        Console.WriteLine($"{i + 1}. {days[i]}");

                    Console.Write("Select day: ");
                    int dayIndex = ReadNumber();

                    if (dayIndex < 1 || dayIndex > days.Length)
                    {
                        Console.WriteLine("Invalid day.");
                        break;
                    }

                    Console.Write("New working hours: ");
                    shop.SetSchedule(days[dayIndex - 1], ReadText());

                    Console.WriteLine("Working hours updated.");
                    break;

                default:
                    Console.WriteLine("Invalid menu option.");
                    break;
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // ==========================================
    // DELETE SHOP
    // ==========================================

    static void DeleteShop()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("Catalog is empty.");
            return;
        }

        ShopUtilities.Print(shops);

        Console.Write("\nEnter shop number to delete: ");
        int index = ReadNumber();

        if (index < 1 || index > shops.Count)
        {
            Console.WriteLine("Invalid shop number.");
            return;
        }

        shops.RemoveAt(index - 1);

        Console.WriteLine("Shop successfully deleted.");
    }

    // ==========================================
    // SORT SHOPS
    // ==========================================

    static void SortShops()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("Catalog is empty.");
            return;
        }

        Console.WriteLine("\nSort by:");
        Console.WriteLine("1. Name");
        Console.WriteLine("2. Address");
        Console.WriteLine("3. Specialization");

        Console.Write("Your choice: ");
        string choice = Console.ReadLine() ?? "";

        MyLinkedList<Shop> sorted;

        switch (choice)
        {
            case "1":
                sorted = ShopUtilities.SortBy(
                    shops,
                    shop => shop.Name,
                    StringComparer.CurrentCultureIgnoreCase);
                break;

            case "2":
                sorted = ShopUtilities.SortBy(
                    shops,
                    shop => shop.Address,
                    StringComparer.CurrentCultureIgnoreCase);
                break;

            case "3":
                sorted = ShopUtilities.SortBy(
                    shops,
                    shop => shop.Specialization,
                    StringComparer.CurrentCultureIgnoreCase);
                break;

            default:
                Console.WriteLine("Invalid menu option.");
                return;
        }

        Console.WriteLine("\n--- Sorted Catalog ---");
        ShopUtilities.Print(sorted);
    }

    // ==========================================
    // SEARCH SHOPS
    // ==========================================

    static void FindShops()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("Catalog is empty.");
            return;
        }

        Console.WriteLine("\nSearch by:");
        Console.WriteLine("1. Name");
        Console.WriteLine("2. Address");
        Console.WriteLine("3. Specialization");

        Console.Write("Your choice: ");
        string choice = Console.ReadLine() ?? "";

        Console.Write("Search query: ");
        string query = ReadText();

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
                    shop => shop.Address.Contains(
                        query,
                        StringComparison.OrdinalIgnoreCase));
                break;

            case "3":
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

    // ==========================================
    // SAVE CATALOG
    // ==========================================

    static void SaveCatalog()
    {
        try
        {
            Console.Write("Enter file path (e.g. shops.json): ");
            string path = ReadText();

            if (!path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                path += ".json";

            SerializationManager.Save(shops, path);

            Console.WriteLine($"File path: {Path.GetFullPath(path)}");
        }
        catch (Exception ex) when (
            ex is IOException ||
            ex is UnauthorizedAccessException ||
            ex is ArgumentException)
        {
            Console.WriteLine($"Save error: {ex.Message}");
        }
    }

    // ==========================================
    // LOAD CATALOG
    // ==========================================

    static void LoadCatalog()
    {
        try
        {
            Console.Write("Enter file path (e.g. shops.json): ");
            string path = ReadText();

            if (!path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                path += ".json";

            MyLinkedList<Shop> loaded =
                SerializationManager.Load(path);

            shops = loaded;

            Console.WriteLine(
                $"Catalog restored. Number of shops: {shops.Count}");
        }
        catch (Exception ex) when (
            ex is IOException ||
            ex is UnauthorizedAccessException ||
            ex is System.Text.Json.JsonException ||
            ex is ArgumentException)
        {
            Console.WriteLine($"Load error: {ex.Message}");
        }
    }

    // ==========================================
    // AUTOMATIC MODE
    // ==========================================

    static void RunAutomaticMode()
    {
        Console.WriteLine("========== AUTOMATIC MODE ==========");

        // Generate test data
        Shop shop1 = new Shop(
            "GameZone",
            "Kyiv, Khreshchatyk Street 15",
            "Video Games");

        shop1.AddPhone("+380501112233");
        shop1.AddPhone("+380671234567");

        shop1.SetSchedule("Monday-Friday", "09:00-20:00");
        shop1.SetSchedule("Saturday", "10:00-18:00");
        shop1.SetSchedule("Sunday", "Closed");

        Shop shop2 = new Shop(
            "TechWorld",
            "Lviv, Svobody Avenue 22",
            "Electronics");

        shop2.AddPhone("+380931234567");

        shop2.SetSchedule("Monday-Friday", "10:00-19:00");
        shop2.SetSchedule("Saturday", "10:00-16:00");
        shop2.SetSchedule("Sunday", "Closed");

        Shop shop3 = new Shop(
            "BookLand",
            "Odesa, Deribasivska Street 10",
            "Books");

        shop3.AddPhone("+380991112233");
        shop3.AddPhone("+380661234567");
        shop3.AddPhone("+380731234567");

        shop3.SetSchedule("Monday-Friday", "09:00-18:00");
        shop3.SetSchedule("Saturday", "10:00-17:00");
        shop3.SetSchedule("Sunday", "Closed");

        shops.Add(shop1);
        shops.Add(shop2);
        shops.Add(shop3);

        Console.WriteLine("\n1. Generated shops:");
        ShopUtilities.Print(shops);

        // Demonstrate sorting
        Console.WriteLine("\n2. Sorting by name:");

        MyLinkedList<Shop> sorted = ShopUtilities.SortBy(
            shops,
            shop => shop.Name,
            StringComparer.OrdinalIgnoreCase);

        ShopUtilities.Print(sorted);

        // Demonstrate searching
        Console.WriteLine("\n3. Searching for Video Games:");

        MyLinkedList<Shop> found = ShopUtilities.Find(
            shops,
            shop => shop.Specialization.Contains(
                "Video Games",
                StringComparison.OrdinalIgnoreCase));

        ShopUtilities.Print(found);

        // Demonstrate serialization
        string filePath = Path.Combine(
            AppContext.BaseDirectory,
            "shops_auto.json");

        Console.WriteLine("\n4. Saving catalog:");

        SerializationManager.Save(shops, filePath);

        Console.WriteLine($"Saved to: {filePath}");

        // Clear and restore
        Console.WriteLine("\n5. Clearing catalog:");

        shops.Clear();

        Console.WriteLine($"Number of shops: {shops.Count}");

        Console.WriteLine("\n6. Restoring catalog:");

        shops = SerializationManager.Load(filePath);

        ShopUtilities.Print(shops);

        // Demonstrate deletion
        Console.WriteLine("\n7. Removing the first shop:");

        shops.RemoveAt(0);

        ShopUtilities.Print(shops);

        Console.WriteLine("\n========== AUTOMATIC MODE FINISHED ==========");
    }

    // ==========================================
    // INPUT METHODS
    // ==========================================

    static string ReadText()
    {
        while (true)
        {
            string input = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.Write("Input cannot be empty. Try again: ");
        }
    }

    static int ReadNumber()
    {
        while (true)
        {
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int number) && number >= 0)
                return number;

            Console.Write("Enter a non-negative integer: ");
        }
    }
}