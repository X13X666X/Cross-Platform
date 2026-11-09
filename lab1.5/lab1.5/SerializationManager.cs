using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ShopCatalog14;

public static class SerializationManager
{
    private static readonly JsonSerializerOptions options =
        new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

    public static void Save(
        MyLinkedList<Shop> shops,
        string filePath)
    {
        Shop[] array = shops.ToArray();

        string json = JsonSerializer.Serialize(array, options);

        File.WriteAllText(
            filePath,
            json,
            new UTF8Encoding(false));

        Console.WriteLine("Catalog successfully saved.");
    }

    public static MyLinkedList<Shop> Load(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException(
                "File not found.", filePath);

        string json = File.ReadAllText(filePath, Encoding.UTF8);

        Shop[] loadedShops =
            JsonSerializer.Deserialize<Shop[]>(json, options);

        if (loadedShops == null)
            throw new InvalidDataException(
                "Could not restore catalog.");

        MyLinkedList<Shop> result = new MyLinkedList<Shop>();

        foreach (Shop shop in loadedShops)
            result.Add(shop);

        return result;
    }
}