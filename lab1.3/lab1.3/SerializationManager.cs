using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ShopCatalog;

public static class SerializationManager
{
    private static readonly JsonSerializerOptions options =
        new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

    public static void Save(
        ShopArrayContainer<Shop> container,
        string filePath)
    {
        Shop[] shops = container.ToArray();

        string json = JsonSerializer.Serialize(shops, options);

        File.WriteAllText(filePath, json, new UTF8Encoding(false));

        Console.WriteLine("Каталог успішно збережено.");
    }

    public static ShopArrayContainer<Shop> Load(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException(
                "Файл не знайдено.", filePath);

        string json = File.ReadAllText(filePath, Encoding.UTF8);

        Shop[] loadedShops =
            JsonSerializer.Deserialize<Shop[]>(json, options);

        if (loadedShops == null)
            throw new InvalidDataException(
                "Не вдалося відновити каталог.");

        ShopArrayContainer<Shop> container =
            new ShopArrayContainer<Shop>();

        foreach (Shop shop in loadedShops)
        {
            container.Add(shop);
        }

        return container;
    }
}