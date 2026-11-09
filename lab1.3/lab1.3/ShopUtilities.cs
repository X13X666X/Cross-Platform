using System;
using System.Collections.Generic;

namespace ShopCatalog;

public static class ShopUtilities
{
    public static ShopArrayContainer<T> SortBy<T, TKey>(
        ShopArrayContainer<T> collection,
        Func<T, TKey> selector,
        IComparer<TKey> comparer = null)
    {
        List<T> temporaryList = new List<T>();

        foreach (T item in collection)
        {
            temporaryList.Add(item);
        }

        comparer ??= Comparer<TKey>.Default;

        temporaryList.Sort((a, b) =>
            comparer.Compare(selector(a), selector(b)));

        ShopArrayContainer<T> result = new ShopArrayContainer<T>();

        foreach (T item in temporaryList)
        {
            result.Add(item);
        }

        return result;
    }

    public static ShopArrayContainer<T> Find<T>(
        ShopArrayContainer<T> collection,
        Predicate<T> condition)
    {
        ShopArrayContainer<T> result = new ShopArrayContainer<T>();

        foreach (T item in collection)
        {
            if (condition(item))
            {
                result.Add(item);
            }
        }

        return result;
    }

    public static void Print<T>(ShopArrayContainer<T> collection)
    {
        if (collection.Count == 0)
        {
            Console.WriteLine("Каталог порожній.");
            return;
        }

        int index = 1;

        foreach (T item in collection)
        {
            Console.WriteLine($"\n========== Магазин #{index} ==========");
            Console.WriteLine(item);
            index++;
        }
    }
}