using System;
using System.Text;

namespace ShopCatalog;

class Program
{
    static ShopArrayContainer<Shop> shops = new ShopArrayContainer<Shop>();

    static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        bool running = true;

        while (running)
        {
            Console.WriteLine("\n========== КАТАЛОГ МАГАЗИНІВ ==========");
            Console.WriteLine("1. Додати магазин");
            Console.WriteLine("2. Переглянути всі магазини");
            Console.WriteLine("3. Редагувати магазин");
            Console.WriteLine("4. Видалити магазин");
            Console.WriteLine("5. Сортувати магазини");
            Console.WriteLine("6. Знайти магазини");
            Console.WriteLine("7. Зберегти каталог");
            Console.WriteLine("8. Завантажити каталог");
            Console.WriteLine("9. Очистити каталог");
            Console.WriteLine("0. Вихід");
            Console.WriteLine("=======================================");

            Console.Write("Ваш вибір: ");
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
                    ClearCatalog();
                    break;

                case "0":
                    running = false;
                    Console.WriteLine("Програму завершено.");
                    break;

                default:
                    Console.WriteLine("Неправильний пункт меню.");
                    break;
            }
        }
    }

    static void AddShop()
    {
        Console.WriteLine("\n--- Додавання магазину ---");

        try
        {
            Console.Write("Назва: ");
            string name = ReadText();

            Console.Write("Адреса: ");
            string address = ReadText();

            Console.Write("Спеціалізація: ");
            string specialization = ReadText();

            Shop shop = new Shop(name, address, specialization);

            Console.Write("\nКількість телефонів: ");
            int phoneCount = ReadNumber();

            for (int i = 0; i < phoneCount; i++)
            {
                Console.Write($"Телефон #{i + 1}: ");
                shop.AddPhone(ReadText());
            }

            string[] days =
            {
                "Понеділок",
                "Вівторок",
                "Середа",
                "Четвер",
                "П'ятниця",
                "Субота",
                "Неділя"
            };

            Console.WriteLine("\nВведіть графік роботи:");

            foreach (string day in days)
            {
                Console.Write($"{day} (наприклад, 09:00-18:00 або Вихідний): ");
                shop.SetSchedule(day, ReadText());
            }

            shops.Add(shop);

            Console.WriteLine("\nМагазин успішно додано.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    static void EditShop()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("Каталог порожній.");
            return;
        }

        ShopUtilities.Print(shops);

        Console.Write("\nВведіть номер магазину для редагування: ");
        int index = ReadNumber();

        if (index < 1 || index > shops.Count)
        {
            Console.WriteLine("Неправильний номер магазину.");
            return;
        }

        Shop shop = shops.Get(index - 1);

        Console.WriteLine("\n1. Змінити основну інформацію");
        Console.WriteLine("2. Додати телефон");
        Console.WriteLine("3. Видалити телефон");
        Console.WriteLine("4. Змінити графік роботи");

        Console.Write("Ваш вибір: ");
        string choice = Console.ReadLine() ?? "";

        try
        {
            switch (choice)
            {
                case "1":
                    Console.Write("Нова назва: ");
                    string name = ReadText();

                    Console.Write("Нова адреса: ");
                    string address = ReadText();

                    Console.Write("Нова спеціалізація: ");
                    string specialization = ReadText();

                    shop.Update(name, address, specialization);

                    Console.WriteLine("Інформацію оновлено.");
                    break;

                case "2":
                    Console.Write("Новий телефон: ");
                    shop.AddPhone(ReadText());

                    Console.WriteLine("Телефон додано.");
                    break;

                case "3":
                    if (shop.Phones.Count == 0)
                    {
                        Console.WriteLine("У магазину немає телефонів.");
                        break;
                    }

                    for (int i = 0; i < shop.Phones.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {shop.Phones[i]}");
                    }

                    Console.Write("Номер телефону для видалення: ");
                    int phoneIndex = ReadNumber();

                    if (phoneIndex < 1 || phoneIndex > shop.Phones.Count)
                    {
                        Console.WriteLine("Неправильний номер.");
                        break;
                    }

                    shop.RemovePhone(phoneIndex - 1);

                    Console.WriteLine("Телефон видалено.");
                    break;

                case "4":
                    string[] days =
                    {
                        "Понеділок",
                        "Вівторок",
                        "Середа",
                        "Четвер",
                        "П'ятниця",
                        "Субота",
                        "Неділя"
                    };

                    for (int i = 0; i < days.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {days[i]}");
                    }

                    Console.Write("Виберіть день: ");
                    int dayIndex = ReadNumber();

                    if (dayIndex < 1 || dayIndex > days.Length)
                    {
                        Console.WriteLine("Неправильний номер дня.");
                        break;
                    }

                    Console.Write("Новий графік роботи: ");
                    string hours = ReadText();

                    shop.SetSchedule(days[dayIndex - 1], hours);

                    Console.WriteLine("Графік роботи оновлено.");
                    break;

                default:
                    Console.WriteLine("Неправильний пункт меню.");
                    break;
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    static void DeleteShop()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("Каталог порожній.");
            return;
        }

        ShopUtilities.Print(shops);

        Console.Write("\nВведіть номер магазину для видалення: ");
        int index = ReadNumber();

        if (index < 1 || index > shops.Count)
        {
            Console.WriteLine("Неправильний номер магазину.");
            return;
        }

        shops.RemoveAt(index - 1);

        Console.WriteLine("Магазин успішно видалено.");
    }

    static void SortShops()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("Каталог порожній.");
            return;
        }

        Console.WriteLine("\nСортувати за:");
        Console.WriteLine("1. Назвою");
        Console.WriteLine("2. Адресою");
        Console.WriteLine("3. Спеціалізацією");

        Console.Write("Ваш вибір: ");
        string choice = Console.ReadLine() ?? "";

        ShopArrayContainer<Shop> sortedShops;

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
                Console.WriteLine("Неправильний пункт меню.");
                return;
        }

        Console.WriteLine("\n--- Відсортований каталог ---");
        ShopUtilities.Print(sortedShops);
    }

    static void FindShops()
    {
        if (shops.Count == 0)
        {
            Console.WriteLine("Каталог порожній.");
            return;
        }

        Console.WriteLine("\nШукати за:");
        Console.WriteLine("1. Назвою");
        Console.WriteLine("2. Спеціалізацією");
        Console.WriteLine("3. Адресою");

        Console.Write("Ваш вибір: ");
        string choice = Console.ReadLine() ?? "";

        Console.Write("Введіть пошуковий запит: ");
        string query = ReadText();

        ShopArrayContainer<Shop> result;

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

            case "3":
                result = ShopUtilities.Find(
                    shops,
                    shop => shop.Address.Contains(
                        query,
                        StringComparison.OrdinalIgnoreCase));
                break;

            default:
                Console.WriteLine("Неправильний пункт меню.");
                return;
        }

        Console.WriteLine("\n--- Результати пошуку ---");
        ShopUtilities.Print(result);
    }

    static void SaveCatalog()
    {
        try
        {
            Console.WriteLine("\nВиберіть директорію для збереження:");

            string directory = DirectoryNavigator.SelectDirectory();

            if (directory == null)
            {
                Console.WriteLine("Збереження скасовано.");
                return;
            }

            Console.Write("Введіть назву файлу (без розширення): ");
            string fileName = ReadText();

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                Console.WriteLine("Назва файлу містить недопустимі символи.");
                return;
            }

            string filePath = Path.Combine(directory, fileName + ".json");

            if (File.Exists(filePath))
            {
                Console.Write("Файл вже існує. Перезаписати? (y/n): ");

                if (!string.Equals(
                    Console.ReadLine(),
                    "y",
                    StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Збереження скасовано.");
                    return;
                }
            }

            SerializationManager.Save(shops, filePath);

            Console.WriteLine($"Шлях до файлу: {filePath}");
        }
        catch (Exception ex) when (
            ex is IOException ||
            ex is UnauthorizedAccessException ||
            ex is ArgumentException)
        {
            Console.WriteLine($"Помилка збереження: {ex.Message}");
        }
    }

    static void LoadCatalog()
    {
        try
        {
            Console.WriteLine("\nВиберіть JSON-файл для завантаження:");

            string filePath = DirectoryNavigator.SelectJsonFile();

            if (filePath == null)
            {
                Console.WriteLine("Завантаження скасовано.");
                return;
            }

            ShopArrayContainer<Shop> loadedShops =
                SerializationManager.Load(filePath);

            shops = loadedShops;

            Console.WriteLine(
                $"Каталог успішно відновлено. Кількість магазинів: {shops.Count}");
        }
        catch (Exception ex) when (
            ex is IOException ||
            ex is UnauthorizedAccessException ||
            ex is System.Text.Json.JsonException ||
            ex is ArgumentException)
        {
            Console.WriteLine($"Помилка завантаження: {ex.Message}");
        }
    }

    static void ClearCatalog()
    {
        Console.Write("Ви впевнені, що хочете очистити каталог? (y/n): ");

        if (string.Equals(
            Console.ReadLine(),
            "y",
            StringComparison.OrdinalIgnoreCase))
        {
            shops.Clear();
            Console.WriteLine("Каталог очищено.");
        }
        else
        {
            Console.WriteLine("Операцію скасовано.");
        }
    }

    static string ReadText()
    {
        while (true)
        {
            string input = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.Write("Поле не може бути порожнім. Спробуйте ще раз: ");
        }
    }

    static int ReadNumber()
    {
        while (true)
        {
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int number) && number >= 0)
                return number;

            Console.Write("Введіть невід'ємне ціле число: ");
        }
    }
}