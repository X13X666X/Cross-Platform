using System;
using System.Collections.Generic;

namespace ShopCatalog14;

public static class ShopUtilities
{
    // Generic sorting method
    public static MyLinkedList<T> SortBy<T, TKey>(
        MyLinkedList<T> collection,
        Func<T, TKey> selector,
        IComparer<TKey> comparer = null)
    {
        T[] array = collection.ToArray();

        comparer ??= Comparer<TKey>.Default;

        // Insertion Sort
        for (int i = 1; i < array.Length; i++)
        {
            T current = array[i];
            int j = i - 1;

            while (j >= 0 &&
                   comparer.Compare(
                       selector(array[j]),
                       selector(current)) > 0)
            {
                array[j + 1] = array[j];
                j--;
            }

            array[j + 1] = current;
        }

        MyLinkedList<T> result = new MyLinkedList<T>();

        for (int i = 0; i < array.Length; i++)
            result.Add(array[i]);

        return result;
    }

    // Generic search method
    public static MyLinkedList<T> Find<T>(
        MyLinkedList<T> collection,
        Predicate<T> condition)
    {
        MyLinkedList<T> result = new MyLinkedList<T>();

        foreach (T item in collection)
        {
            if (condition(item))
                result.Add(item);
        }

        return result;
    }

    // Generic output method
    public static void Print<T>(MyLinkedList<T> collection)
    {
        if (collection.Count == 0)
        {
            Console.WriteLine("Каталог порожній.");
            return;
        }

        int index = 1;

        foreach (T item in collection)
        {
            Console.WriteLine($"\n========== Запис #{index} ==========");
            Console.WriteLine(item);
            index++;
        }
    }
}