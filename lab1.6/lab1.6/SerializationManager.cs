using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ShopCatalog16;

public static class SerializationManager
{
    private static readonly JsonSerializerOptions Options =
        new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

    public static void Save(
        List<Shop> shops,
        string filePath)
    {
        string json = JsonSerializer.Serialize(shops, Options);

        File.WriteAllText(filePath, json);

        Console.WriteLine("Catalog successfully saved.");
    }

    public static List<Shop> Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "File not found.",
                filePath);
        }

        string json = File.ReadAllText(filePath);

        List<Shop> shops =
            JsonSerializer.Deserialize<List<Shop>>(json, Options);

        if (shops == null)
        {
            throw new InvalidDataException(
                "Could not load catalog.");
        }

        Console.WriteLine("Catalog successfully loaded.");

        return shops;
    }
}