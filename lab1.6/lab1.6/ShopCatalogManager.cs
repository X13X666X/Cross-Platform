using System;
using System.Collections.Generic;

namespace ShopCatalog16;

public class ShopCatalogManager
{
    public List<Shop> Shops { get; private set; }

    public ShopCatalogManager()
    {
        Shops = new List<Shop>();
    }

    public void AddShop(Shop shop)
    {
        Shops.Add(shop);
    }

    public bool RemoveShop(int index)
    {
        if (index < 0 || index >= Shops.Count)
            return false;

        Shops.RemoveAt(index);
        return true;
    }

    public List<Shop> FindByName(string name)
    {
        return Shops.FindAll(shop =>
            shop.Name.Contains(
                name,
                StringComparison.OrdinalIgnoreCase));
    }

    public List<Shop> FindByAddress(string address)
    {
        return Shops.FindAll(shop =>
            shop.Address.Contains(
                address,
                StringComparison.OrdinalIgnoreCase));
    }

    public List<Shop> FindBySpecialization(string specialization)
    {
        return Shops.FindAll(shop =>
            shop.Specialization.Contains(
                specialization,
                StringComparison.OrdinalIgnoreCase));
    }

    public void SortByName()
    {
        Shops.Sort((first, second) =>
            string.Compare(
                first.Name,
                second.Name,
                StringComparison.CurrentCultureIgnoreCase));
    }

    public void SortByAddress()
    {
        Shops.Sort((first, second) =>
            string.Compare(
                first.Address,
                second.Address,
                StringComparison.CurrentCultureIgnoreCase));
    }

    public void SortBySpecialization()
    {
        Shops.Sort((first, second) =>
            string.Compare(
                first.Specialization,
                second.Specialization,
                StringComparison.CurrentCultureIgnoreCase));
    }

    public void ReplaceShops(List<Shop> newShops)
    {
        Shops = newShops ?? new List<Shop>();
    }

    public void DisplayAll()
    {
        if (Shops.Count == 0)
        {
            Console.WriteLine("Catalog is empty.");
            return;
        }

        for (int i = 0; i < Shops.Count; i++)
        {
            Console.WriteLine($"\n========== SHOP #{i + 1} ==========");
            Console.WriteLine(Shops[i]);
        }
    }

    public void DisplayResults(List<Shop> results)
    {
        if (results.Count == 0)
        {
            Console.WriteLine("No shops found.");
            return;
        }

        foreach (Shop shop in results)
        {
            Console.WriteLine("\n----------------------------");
            Console.WriteLine(shop);
        }
    }
}