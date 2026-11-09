using System;
using System.IO;
using System.Text;

namespace ShopCatalog14;

internal class Program
{
    private static MyLinkedList<Shop> shops =
        new MyLinkedList<Shop>();

    private static ShopSearchCriteria criteria =
        new ShopSearchCriteria();

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
            Console.WriteLine("3. Edit shop");
            Console.WriteLine("4. Delete shop");
            Console.WriteLine("5. Sort shops");
            Console.WriteLine("6. Search by variant criteria");
            Console.WriteLine("7. Change search criteria");
            Console.WriteLine("8. Save catalog");
            Console.WriteLine("9. Load catalog");
            Console.WriteLine("10. Clear catalog");
            Console.WriteLine("0. Exit");

            int choice = ReadInt(
                "Choose an option: ", 0, 10);

            try
            {
                switch (choice)
                {
                    case 1:
                        AddShop();
                        break;

                    case 2:
                        ShopUtilities.Print(shops);
                        break;

                    case 3:
                        EditShop();
                        break;

                    case 4:
                        DeleteShop();
                        break;

                    case 5:
                        SortShops();
                        break;

                    case 6:
                        SearchShops();
                        break;

                    case 7:
                        ChangeSearchCriteria();
                        break;

                    case 8:
                        SaveCatalog();
                        break;

                    case 9:
                        LoadCatalog();
                        break;

                    case 10:
                        shops.Clear();
                        Console.WriteLine("Catalog cleared.");
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
    }

    // ==================== ADD SHOP ====================

    private static void AddShop()
    {
        Console.WriteLine("\n========== ADD SHOP ==========");

        string name = ReadValidated(
            "Shop name: ",
            RegexValidator.IsValidName,
            "Invalid name.");

        string address = ReadValidated(
            "Address: ",
            RegexValidator.IsValidAddress,
            "Invalid address.");

        string specialization = ReadValidated(
            "Specialization: ",
            RegexValidator.IsValidName,
            "Invalid specialization.");

        Shop shop = new Shop(
            name,
            address,
            specialization);

        int phoneCount = ReadInt(
            "Number of phones: ", 0, 100);

        for (int i = 0; i < phoneCount; i++)
        {
            string phone = ReadValidated(
                $"Phone #{i + 1}: ",
                RegexValidator.IsValidPhone,
                "Invalid Ukrainian mobile phone number.");

            shop.AddPhone(phone);
        }

        Console.WriteLine("\nEnter working hours for each day.");
        Console.WriteLine(
            "Format: HH:mm-HH:mm or HH:mm-HH:mm,HH:mm-HH:mm or Closed");

        foreach (string day in Days)
        {
            string hours = ReadValidated(
                day + ": ",
                RegexValidator.IsValidScheduleEntry,
                "Invalid working schedule.");

            shop.SetSchedule(day, hours);
        }

        shops.Add(shop);

        Console.WriteLine("Shop successfully added.");
    }

    // ==================== EDIT SHOP ====================

    private static void EditShop()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("Catalog is empty.");
            return;
        }

        ShopUtilities.Print(shops);

        int index = ReadInt(
            "Enter shop number: ", 1, shops.Count) - 1;

        Shop shop = shops.Get(index);

        Console.WriteLine("\nEditing: " + shop.Name);

        string name = ReadValidated(
            "New name: ",
            RegexValidator.IsValidName,
            "Invalid name.");

        string address = ReadValidated(
            "New address: ",
            RegexValidator.IsValidAddress,
            "Invalid address.");

        string specialization = ReadValidated(
            "New specialization: ",
            RegexValidator.IsValidName,
            "Invalid specialization.");

        shop.Update(name, address, specialization);

        Console.WriteLine("\nPhone management:");
        Console.WriteLine("1. Add phone");
        Console.WriteLine("2. Remove phone");
        Console.WriteLine("0. Skip");

        int phoneChoice = ReadInt("Choose: ", 0, 2);

        if (phoneChoice == 1)
        {
            string phone = ReadValidated(
                "New phone: ",
                RegexValidator.IsValidPhone,
                "Invalid phone.");

            shop.AddPhone(phone);
        }
        else if (phoneChoice == 2)
        {
            if (shop.Phones.Length == 0)
            {
                Console.WriteLine("No phones.");
            }
            else
            {
                for (int i = 0; i < shop.Phones.Length; i++)
                    Console.WriteLine(
                        $"{i + 1}. {shop.Phones[i]}");

                int phoneIndex = ReadInt(
                    "Phone number to remove: ",
                    1,
                    shop.Phones.Length);

                shop.RemovePhone(phoneIndex - 1);
            }
        }

        Console.WriteLine("\nSchedule management:");

        for (int i = 0; i < Days.Length; i++)
            Console.WriteLine($"{i + 1}. {Days[i]}");

        int dayChoice = ReadInt(
            "Choose day to change: ", 1, 7);

        string newHours = ReadValidated(
            "New hours: ",
            RegexValidator.IsValidScheduleEntry,
            "Invalid working schedule.");

        shop.SetSchedule(
            Days[dayChoice - 1],
            newHours);

        Console.WriteLine("Shop updated.");
    }

    // ==================== DELETE SHOP ====================

    private static void DeleteShop()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("Catalog is empty.");
            return;
        }

        ShopUtilities.Print(shops);

        int index = ReadInt(
            "Enter shop number to delete: ",
            1,
            shops.Count);

        shops.RemoveAt(index - 1);

        Console.WriteLine("Shop deleted.");
    }

    // ==================== SORT ====================

    private static void SortShops()
    {
        Console.WriteLine("\nSort by:");
        Console.WriteLine("1. Name");
        Console.WriteLine("2. Address");
        Console.WriteLine("3. Specialization");

        int choice = ReadInt("Choose: ", 1, 3);

        switch (choice)
        {
            case 1:
                shops = ShopUtilities.SortBy(
                    shops,
                    shop => shop.Name,
                    StringComparer.CurrentCultureIgnoreCase);
                break;

            case 2:
                shops = ShopUtilities.SortBy(
                    shops,
                    shop => shop.Address,
                    StringComparer.CurrentCultureIgnoreCase);
                break;

            case 3:
                shops = ShopUtilities.SortBy(
                    shops,
                    shop => shop.Specialization,
                    StringComparer.CurrentCultureIgnoreCase);
                break;
        }

        Console.WriteLine("Catalog sorted.");

        ShopUtilities.Print(shops);
    }

    // ==================== SEARCH ====================

    private static void SearchShops()
    {
        Console.WriteLine("\n========== SEARCH RESULTS ==========");

        MyLinkedList<Shop> result =
            ShopUtilities.FindShopsByCriteria(
                shops,
                criteria);

        Console.WriteLine($"Found shops: {result.Count}");

        ShopUtilities.Print(result);
    }

    // ==================== CHANGE CRITERIA ====================

    private static void ChangeSearchCriteria()
    {
        Console.WriteLine("\n========== SEARCH CRITERIA ==========");

        Console.WriteLine(
            $"1. Exclude food stores: {criteria.ExcludeFoodStores}");

        Console.WriteLine(
            $"2. Require all 7 days: {criteria.RequireAllDays}");

        Console.WriteLine(
            $"3. Require no lunch break: {criteria.RequireNoLunchBreak}");

        Console.WriteLine(
            $"4. Require Ukrainian mobile phone: {criteria.RequireUkrainianMobilePhone}");

        Console.WriteLine("0. Return");

        int choice = ReadInt(
            "Choose criterion to toggle: ", 0, 4);

        switch (choice)
        {
            case 1:
                criteria.ExcludeFoodStores =
                    !criteria.ExcludeFoodStores;
                break;

            case 2:
                criteria.RequireAllDays =
                    !criteria.RequireAllDays;
                break;

            case 3:
                criteria.RequireNoLunchBreak =
                    !criteria.RequireNoLunchBreak;
                break;

            case 4:
                criteria.RequireUkrainianMobilePhone =
                    !criteria.RequireUkrainianMobilePhone;
                break;
        }

        Console.WriteLine("Search criteria updated.");
    }

    // ==================== SAVE / LOAD ====================

    private static void SaveCatalog()
    {
        string path = ReadText("Enter file path: ");

        if (!path.EndsWith(
            ".json",
            StringComparison.OrdinalIgnoreCase))
        {
            path += ".json";
        }

        SerializationManager.Save(shops, path);
    }

    private static void LoadCatalog()
    {
        string path = ReadText("Enter file path: ");

        if (!path.EndsWith(
            ".json",
            StringComparison.OrdinalIgnoreCase))
        {
            path += ".json";
        }

        shops = SerializationManager.Load(path);

        Console.WriteLine("Catalog loaded.");
    }

    // ==================== AUTOMATIC MODE ====================

    private static void RunAutomaticMode()
    {
        Console.WriteLine("========== AUTOMATIC MODE ==========");

        shops = new MyLinkedList<Shop>();

        CreateAutomaticData();

        Console.WriteLine("\n1. Initial catalog:");

        ShopUtilities.Print(shops);

        Console.WriteLine("\n2. Regular expression tests:");

        TestPhone("+380501234567");
        TestPhone("0501234567");
        TestPhone("0931234567");
        TestPhone("1234567890");
        TestPhone("+380001234567");

        Console.WriteLine(
            "\nValid name: " +
            RegexValidator.IsValidName("GameZone"));

        Console.WriteLine(
            "Invalid name: " +
            RegexValidator.IsValidName("Game@Zone"));

        Console.WriteLine(
            "Valid hours: " +
            RegexValidator.IsValidWorkingHours("09:00-20:00"));

        Console.WriteLine(
            "Invalid hours: " +
            RegexValidator.IsValidWorkingHours("25:00-20:00"));

        Console.WriteLine("\n3. Search with default criteria:");

        criteria = new ShopSearchCriteria();

        MyLinkedList<Shop> result =
            ShopUtilities.FindShopsByCriteria(
                shops,
                criteria);

        Console.WriteLine($"Found shops: {result.Count}");

        ShopUtilities.Print(result);

        Console.WriteLine("\n4. Change search criteria:");

        criteria.RequireUkrainianMobilePhone = false;

        Console.WriteLine(
            "Ukrainian mobile phone requirement disabled.");

        result = ShopUtilities.FindShopsByCriteria(
            shops,
            criteria);

        Console.WriteLine(
            $"Found shops after changing criteria: {result.Count}");

        ShopUtilities.Print(result);

        Console.WriteLine("\n5. Sort shops by name:");

        shops = ShopUtilities.SortBy(
            shops,
            shop => shop.Name,
            StringComparer.CurrentCultureIgnoreCase);

        ShopUtilities.Print(shops);

        Console.WriteLine("\n6. Save and load catalog:");

        string filePath = Path.Combine(
            AppContext.BaseDirectory,
            "shops_lab15.json");

        SerializationManager.Save(shops, filePath);

        shops.Clear();

        Console.WriteLine("Catalog cleared from memory.");

        shops = SerializationManager.Load(filePath);

        Console.WriteLine("Catalog restored from JSON.");

        ShopUtilities.Print(shops);

        Console.WriteLine(
            "\n========== AUTOMATIC MODE FINISHED ==========");

        Console.WriteLine("File: " + filePath);
    }

    // ==================== TEST DATA ====================

    private static void CreateAutomaticData()
    {
        // 1. This shop meets all default criteria.

        Shop gameZone = new Shop(
            "GameZone",
            "Kyiv, Khreshchatyk Street 15",
            "Video Games");

        gameZone.AddPhone("+380501112233");
        gameZone.AddPhone("+380671234567");

        SetFullWeek(gameZone, "09:00-20:00");

        shops.Add(gameZone);

        // 2. This shop is closed on Sunday.

        Shop techWorld = new Shop(
            "TechWorld",
            "Lviv, Svobody Avenue 22",
            "Electronics");

        techWorld.AddPhone("+380931234567");

        SetFullWeek(techWorld, "10:00-19:00");

        techWorld.SetSchedule("Sunday", "Closed");

        shops.Add(techWorld);

        // 3. This shop sells food.

        Shop foodMarket = new Shop(
            "FoodMarket",
            "Odesa, Deribasivska Street 10",
            "Grocery Store");

        foodMarket.AddPhone("+380991112233");

        SetFullWeek(foodMarket, "08:00-21:00");

        shops.Add(foodMarket);

        // 4. This shop has a lunch break.

        Shop bookLand = new Shop(
            "BookLand",
            "Dnipro, Central Avenue 30",
            "Books");

        bookLand.AddPhone("+380661234567");

        SetFullWeek(
            bookLand,
            "09:00-13:00,14:00-18:00");

        shops.Add(bookLand);

        // 5. This shop has no phone number.

        Shop sportWorld = new Shop(
            "SportWorld",
            "Kharkiv, Sumska Street 45",
            "Sports Equipment");

        SetFullWeek(sportWorld, "10:00-20:00");

        shops.Add(sportWorld);
    }

    private static void SetFullWeek(
        Shop shop,
        string hours)
    {
        foreach (string day in Days)
            shop.SetSchedule(day, hours);
    }

    private static void TestPhone(string phone)
    {
        Console.WriteLine(
            $"{phone} -> {RegexValidator.IsValidPhone(phone)}");
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

    private static string ReadValidated(
        string message,
        Func<string, bool> validator,
        string errorMessage)
    {
        while (true)
        {
            Console.Write(message);

            string value = Console.ReadLine() ?? "";

            if (validator(value))
                return value.Trim();

            Console.WriteLine(errorMessage);
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