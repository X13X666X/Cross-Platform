
using System;
using System.Collections.Generic;

namespace ShopCatalog;

public static class ShopUtilities
{
    public static MyLinkedList<T> SortBy<T, TKey>(
        MyLinkedList<T> collection,
        Func<T, TKey> selector,
        IComparer<TKey> comparer = null)
    {
        List<T> temporaryList = new List<T>();

        foreach (T item in collection)
        {
            temporaryList.Add(item);
        }

        if (comparer == null)
            comparer = Comparer<TKey>.Default;

        temporaryList.Sort((a, b) =>
            comparer.Compare(selector(a), selector(b)));

        MyLinkedList<T> result = new MyLinkedList<T>();

        foreach (T item in temporaryList)
        {
            result.Add(item);
        }

        return result;
    }

    public static MyLinkedList<T> Find<T>(
        MyLinkedList<T> collection,
        Predicate<T> condition)
    {
        MyLinkedList<T> result = new MyLinkedList<T>();

        foreach (T item in collection)
        {
            if (condition(item))
            {
                result.Add(item);
            }
        }

        return result;
    }

    public static void Print<T>(MyLinkedList<T> collection)
    {
        if (collection.Count == 0)
        {
            Console.WriteLine("The collection is empty.");
            return;
        }

        int index = 1;

        foreach (T item in collection)
        {
            Console.WriteLine($"\n--- Record #{index} ---");
            Console.WriteLine(item);
            index++;
        }
    }
}
