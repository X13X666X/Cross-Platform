using System;
using System.IO;
using System.Linq;

namespace ShopCatalog;

public static class DirectoryNavigator
{
    private static string currentDirectory =
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public static string SelectDirectory()
    {
        while (true)
        {
            Console.WriteLine("\n========== ВИБІР ДИРЕКТОРІЇ ==========");
            Console.WriteLine($"Поточна директорія: {currentDirectory}");

            string[] directories = GetDirectories();

            for (int i = 0; i < directories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. [ПАПКА] {Path.GetFileName(directories[i])}");
            }

            Console.WriteLine("\n0. Назад до батьківської директорії");
            Console.WriteLine("S. Вибрати поточну директорію");
            Console.WriteLine("Q. Скасувати");

            Console.Write("\nВаш вибір: ");
            string choice = Console.ReadLine() ?? "";

            if (choice.Equals("Q", StringComparison.OrdinalIgnoreCase))
                return null;

            if (choice.Equals("S", StringComparison.OrdinalIgnoreCase))
                return currentDirectory;

            if (choice == "0")
            {
                GoToParent();
                continue;
            }

            if (int.TryParse(choice, out int index) &&
                index >= 1 && index <= directories.Length)
            {
                currentDirectory = directories[index - 1];
            }
            else
            {
                Console.WriteLine("Неправильний вибір.");
            }
        }
    }

    public static string SelectJsonFile()
    {
        while (true)
        {
            Console.WriteLine("\n========== ВИБІР ФАЙЛУ ==========");
            Console.WriteLine($"Поточна директорія: {currentDirectory}");

            string[] directories = GetDirectories();

            string[] files;

            try
            {
                files = Directory.GetFiles(currentDirectory, "*.json");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Немає доступу до цієї директорії.");
                GoToParent();
                continue;
            }

            Console.WriteLine("\n--- Папки ---");

            for (int i = 0; i < directories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. [ПАПКА] {Path.GetFileName(directories[i])}");
            }

            Console.WriteLine("\n--- JSON-файли ---");

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"{directories.Length + i + 1}. [ФАЙЛ] {Path.GetFileName(files[i])}");
            }

            Console.WriteLine("\n0. Назад до батьківської директорії");
            Console.WriteLine("Q. Скасувати");

            Console.Write("\nВаш вибір: ");
            string choice = Console.ReadLine() ?? "";

            if (choice.Equals("Q", StringComparison.OrdinalIgnoreCase))
                return null;

            if (choice == "0")
            {
                GoToParent();
                continue;
            }

            if (int.TryParse(choice, out int index))
            {
                if (index >= 1 && index <= directories.Length)
                {
                    currentDirectory = directories[index - 1];
                    continue;
                }

                int fileIndex = index - directories.Length - 1;

                if (fileIndex >= 0 && fileIndex < files.Length)
                {
                    return files[fileIndex];
                }
            }

            Console.WriteLine("Неправильний вибір.");
        }
    }

    private static string[] GetDirectories()
    {
        try
        {
            return Directory.GetDirectories(currentDirectory)
                .OrderBy(path => Path.GetFileName(path))
                .ToArray();
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Немає доступу до директорії.");
            return Array.Empty<string>();
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Директорію не знайдено.");
            currentDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            return Array.Empty<string>();
        }
    }

    private static void GoToParent()
    {
        DirectoryInfo parent = Directory.GetParent(currentDirectory);

        if (parent != null)
        {
            currentDirectory = parent.FullName;
        }
        else
        {
            Console.WriteLine("Ви вже перебуваєте в кореневій директорії.");
        }
    }
}