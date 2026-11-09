using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShopCatalog16;

internal class Program
{
    private static ShopCatalogManager catalog =
        new ShopCatalogManager();

    private static readonly string[] Days =
    {
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday",
        "Sunday"
    };

    static void Main(string[] args)
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        if (args.Length > 0 &&
            args[0].Equals(
                "-auto",
                StringComparison.OrdinalIgnoreCase))
        {
            RunAutomaticMode();
        }
        else
        {
            RunDialogMode();
        }
    }

    // ==================== DIALOG MODE ====================

    private static void RunDialogMode()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n========== SHOP CATALOG ==========");
            Console.WriteLine("1. Add shop");
            Console.WriteLine("2. Display all shops");
            Console.WriteLine("3. Search shops");
            Console.WriteLine("4. Edit shop");
            Console.WriteLine("5. Delete shop");
            Console.WriteLine("6. Sort shops");
            Console.WriteLine("7. Save catalog");
            Console.WriteLine("8. Load catalog");
            Console.WriteLine("0. Exit");

            int choice = ReadInt("Choose an option: ", 0, 8);

            try
            {
                switch (choice)
                {
                    case 1:
                        AddShop();
                        break;

                    case 2:
                        catalog.DisplayAll();
                        break;

                    case 3:
                        SearchShops();
                        break;

                    case 4:
                        EditShop();
                        break;

                    case 5:
                        DeleteShop();
                        break;

                    case 6:
                        SortShops();
                        break;

                    case 7:
                        SaveCatalog();
                        break;

                    case 8:
                        LoadCatalog();
                        break;

                    case 0:
                        running = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        Console.WriteLine("Program finished.");
    }

    // ==================== ADD SHOP ====================

    private static void AddShop()
    {
        Console.WriteLine("\n========== ADD SHOP ==========");

        string name = ReadText("Shop name: ");
        string address = ReadText("Address: ");
        string specialization = ReadText("Specialization: ");

        Shop shop = new Shop(
            name,
            address,
            specialization);

        Console.WriteLine("\nAdding phone numbers.");
        Console.WriteLine("Leave the field empty to finish.");

        while (true)
        {
            Console.Write("Phone: ");

            string phone = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(phone))
                break;

            shop.AddPhone(phone);
        }

        Console.WriteLine("\nAdding working schedule.");

        foreach (string day in Days)
        {
            Console.Write($"{day} (e.g. 09:00-18:00 or Closed): ");

            string hours = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(hours))
                hours = "Not specified";

            shop.AddWorkingHours(day, hours);
        }

        catalog.AddShop(shop);

        Console.WriteLine("\nShop successfully added.");
    }

    // ==================== SEARCH ====================

    private static void SearchShops()
    {
        Console.WriteLine("\n========== SEARCH ==========");
        Console.WriteLine("1. Search by name");
        Console.WriteLine("2. Search by address");
        Console.WriteLine("3. Search by specialization");

        int choice = ReadInt("Choose search type: ", 1, 3);

        string query = ReadText("Enter search text: ");

        List<Shop> results;

        switch (choice)
        {
            case 1:
                results = catalog.FindByName(query);
                break;

            case 2:
                results = catalog.FindByAddress(query);
                break;

            default:
                results = catalog.FindBySpecialization(query);
                break;
        }

        Console.WriteLine($"\nFound shops: {results.Count}");

        catalog.DisplayResults(results);
    }

    // ==================== EDIT SHOP ====================

    private static void EditShop()
    {
        if (catalog.Shops.Count == 0)
        {
            Console.WriteLine("Catalog is empty.");
            return;
        }

        catalog.DisplayAll();

        int index = ReadInt(
            "Enter shop number: ",
            1,
            catalog.Shops.Count) - 1;

        Shop shop = catalog.Shops[index];

        Console.WriteLine("\nEditing shop: " + shop.Name);

        shop.Name = ReadText("New name: ");
        shop.Address = ReadText("New address: ");
        shop.Specialization = ReadText("New specialization: ");

        Console.WriteLine("\nPhone numbers:");
        Console.WriteLine("1. Add phone");
        Console.WriteLine("2. Remove phone");
        Console.WriteLine("0. Skip");

        int phoneChoice = ReadInt("Choose: ", 0, 2);

        if (phoneChoice == 1)
        {
            string phone = ReadText("Enter phone: ");
            shop.AddPhone(phone);
        }
        else if (phoneChoice == 2)
        {
            if (shop.Phones.Count == 0)
            {
                Console.WriteLine("No phones available.");
            }
            else
            {
                for (int i = 0; i < shop.Phones.Count; i++)
                {
                    Console.WriteLine(
                        $"{i + 1}. {shop.Phones[i]}");
                }

                int phoneIndex = ReadInt(
                    "Enter phone number to remove: ",
                    1,
                    shop.Phones.Count);

                shop.RemovePhone(phoneIndex - 1);
            }
        }

        Console.WriteLine("\nChange working schedule?");
        Console.WriteLine("1. Yes");
        Console.WriteLine("0. No");

        int scheduleChoice = ReadInt("Choose: ", 0, 1);

        if (scheduleChoice == 1)
        {
            for (int i = 0; i < Days.Length; i++)
                Console.WriteLine($"{i + 1}. {Days[i]}");

            int dayChoice = ReadInt(
                "Choose day: ",
                1,
                Days.Length);

            string hours = ReadText(
                "Enter new working hours: ");

            shop.AddWorkingHours(
                Days[dayChoice - 1],
                hours);
        }

        Console.WriteLine("Shop successfully updated.");
    }

    // ==================== DELETE SHOP ====================

    private static void DeleteShop()
    {
        if (catalog.Shops.Count == 0)
        {
            Console.WriteLine("Catalog is empty.");
            return;
        }

        catalog.DisplayAll();

        int index = ReadInt(
            "Enter shop number to delete: ",
            1,
            catalog.Shops.Count);

        bool removed = catalog.RemoveShop(index - 1);

        Console.WriteLine(
            removed
                ? "Shop successfully deleted."
                : "Shop not found.");
    }

    // ==================== SORT ====================

    private static void SortShops()
    {
        Console.WriteLine("\n========== SORT ==========");
        Console.WriteLine("1. By name");
        Console.WriteLine("2. By address");
        Console.WriteLine("3. By specialization");

        int choice = ReadInt("Choose sorting type: ", 1, 3);

        switch (choice)
        {
            case 1:
                catalog.SortByName();
                break;

            case 2:
                catalog.SortByAddress();
                break;

            case 3:
                catalog.SortBySpecialization();
                break;
        }

        Console.WriteLine("Catalog successfully sorted.");

        catalog.DisplayAll();
    }

    // ==================== SAVE ====================

    private static void SaveCatalog()
    {
        string path = ReadText(
            "Enter file path: ");

        if (!path.EndsWith(
            ".json",
            StringComparison.OrdinalIgnoreCase))
        {
            path += ".json";
        }

        SerializationManager.Save(
            catalog.Shops,
            path);
    }

    // ==================== LOAD ====================

    private static void LoadCatalog()
    {
        string path = ReadText(
            "Enter file path: ");

        if (!path.EndsWith(
            ".json",
            StringComparison.OrdinalIgnoreCase))
        {
            path += ".json";
        }

        List<Shop> loadedShops =
            SerializationManager.Load(path);

        catalog.ReplaceShops(loadedShops);
    }

    // ==================== AUTOMATIC MODE ====================

    private static void RunAutomaticMode()
    {
        Console.WriteLine("========== AUTOMATIC MODE ==========");

        catalog = new ShopCatalogManager();

        CreateTestData();

        Console.WriteLine("\n1. Initial catalog:");

        catalog.DisplayAll();

        // Search
        Console.WriteLine("\n2. Search by specialization: Electronics");

        List<Shop> searchResults =
            catalog.FindBySpecialization("Electronics");

        catalog.DisplayResults(searchResults);

        // Sort by name
        Console.WriteLine("\n3. Sort by name:");

        catalog.SortByName();

        catalog.DisplayAll();

        // Sort by address
        Console.WriteLine("\n4. Sort by address:");

        catalog.SortByAddress();

        catalog.DisplayAll();

        // Sort by specialization
        Console.WriteLine("\n5. Sort by specialization:");

        catalog.SortBySpecialization();

        catalog.DisplayAll();

        // Delete
        Console.WriteLine("\n6. Delete the second shop:");

        if (catalog.Shops.Count > 1)
            catalog.RemoveShop(1);

        catalog.DisplayAll();

        // Save
        Console.WriteLine("\n7. Save catalog:");

        string filePath = Path.Combine(
            AppContext.BaseDirectory,
            "shops_lab16.json");

        SerializationManager.Save(
            catalog.Shops,
            filePath);

        // Clear
        Console.WriteLine("\n8. Clear catalog:");

        catalog.ReplaceShops(new List<Shop>());

        catalog.DisplayAll();

        // Load
        Console.WriteLine("\n9. Load catalog:");

        List<Shop> loadedShops =
            SerializationManager.Load(filePath);

        catalog.ReplaceShops(loadedShops);

        catalog.DisplayAll();

        Console.WriteLine(
            "\n========== AUTOMATIC MODE FINISHED ==========");

        Console.WriteLine("Saved file: " + filePath);
    }

    // ==================== TEST DATA ====================

    private static void CreateTestData()
    {
        Shop shop1 = new Shop(
            "GameZone",
            "Kyiv, Khreshchatyk Street 15",
            "Video Games");

        shop1.AddPhone("+380501112233");
        shop1.AddPhone("+380671234567");

        SetSchedule(shop1, "09:00-20:00");

        catalog.AddShop(shop1);

        Shop shop2 = new Shop(
            "TechWorld",
            "Lviv, Svobody Avenue 22",
            "Electronics");

        shop2.AddPhone("+380931234567");

        SetSchedule(shop2, "10:00-19:00");

        catalog.AddShop(shop2);

        Shop shop3 = new Shop(
            "BookLand",
            "Dnipro, Central Avenue 30",
            "Books");

        bookLandPhone(shop3);

        SetSchedule(shop3, "09:00-18:00");

        catalog.AddShop(shop3);

        Shop shop4 = new Shop(
            "SportWorld",
            "Odesa, Deribasivska Street 10",
            "Sports Equipment");

        shop4.AddPhone("+380991112233");

        SetSchedule(shop4, "10:00-20:00");

        catalog.AddShop(shop4);
    }

    private static void bookLandPhone(Shop shop)
    {
        shop.AddPhone("+380661234567");
    }

    private static void SetSchedule(
        Shop shop,
        string hours)
    {
        foreach (string day in Days)
        {
            shop.AddWorkingHours(day, hours);
        }
    }

    // ==================== INPUT HELPERS ====================

    private static string ReadText(string message)
    {
        while (true)
        {
            Console.Write(message);

            string value = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();

            Console.WriteLine("Value cannot be empty.");
        }
    }

    private static int ReadInt(
        string message,
        int min,
        int max)
    {
        while (true)
        {
            Console.Write(message);

            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int value) &&
                value >= min &&
                value <= max)
            {
                return value;
            }

            Console.WriteLine(
                $"Enter a number from {min} to {max}.");
        }
    }
}