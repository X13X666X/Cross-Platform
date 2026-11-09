using System;
using System.Collections.Generic;

namespace ShopCatalog14;

public static class ShopUtilities
{
    private static readonly string[] RequiredDays =
    {
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday",
        "Sunday"
    };

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

    public static MyLinkedList<Shop> FindShopsByCriteria(
        MyLinkedList<Shop> shops,
        ShopSearchCriteria criteria)
    {
        return Find(
            shops,
            shop => MatchesCriteria(shop, criteria));
    }

    private static bool MatchesCriteria(
        Shop shop,
        ShopSearchCriteria criteria)
    {
        if (criteria.ExcludeFoodStores &&
            RegexValidator.IsFoodSpecialization(shop.Specialization))
        {
            return false;
        }

        if (criteria.RequireAllDays && !WorksAllDays(shop))
            return false;

        if (criteria.RequireNoLunchBreak && !HasNoLunchBreak(shop))
            return false;

        if (criteria.RequireUkrainianMobilePhone &&
            !HasUkrainianMobilePhone(shop))
        {
            return false;
        }

        return true;
    }

    private static bool WorksAllDays(Shop shop)
    {
        foreach (string requiredDay in RequiredDays)
        {
            bool dayFound = false;

            foreach (WorkingHours item in shop.Schedule)
            {
                if (IsSameDay(item.Day, requiredDay) &&
                    !RegexValidator.IsClosedDay(item.Hours))
                {
                    dayFound = true;
                    break;
                }
            }

            if (!dayFound)
                return false;
        }

        return true;
    }

    private static bool HasNoLunchBreak(Shop shop)
    {
        foreach (string requiredDay in RequiredDays)
        {
            bool dayFound = false;

            foreach (WorkingHours item in shop.Schedule)
            {
                if (IsSameDay(item.Day, requiredDay))
                {
                    dayFound = true;

                    // Exactly one continuous interval is required.
                    if (!RegexValidator.IsValidWorkingHours(item.Hours))
                        return false;

                    break;
                }
            }

            if (!dayFound)
                return false;
        }

        return true;
    }

    private static bool HasUkrainianMobilePhone(Shop shop)
    {
        foreach (string phone in shop.Phones)
        {
            if (RegexValidator.IsValidPhone(phone))
                return true;
        }

        return false;
    }

    private static bool IsSameDay(
        string actualDay,
        string requiredDay)
    {
        string normalized = actualDay.Trim().ToLowerInvariant();

        string[] aliases;

        switch (requiredDay)
        {
            case "Monday":
                aliases = new[]
                {
                    "monday", "mon", "понеділок", "понедельник"
                };
                break;

            case "Tuesday":
                aliases = new[]
                {
                    "tuesday", "tue", "вівторок", "вторник"
                };
                break;

            case "Wednesday":
                aliases = new[]
                {
                    "wednesday", "wed", "середа", "среда"
                };
                break;

            case "Thursday":
                aliases = new[]
                {
                    "thursday", "thu", "четвер", "четверг"
                };
                break;

            case "Friday":
                aliases = new[]
                {
                    "friday", "fri", "п'ятниця", "пятница"
                };
                break;

            case "Saturday":
                aliases = new[]
                {
                    "saturday", "sat", "субота", "суббота"
                };
                break;

            default:
                aliases = new[]
                {
                    "sunday", "sun", "неділя", "воскресенье"
                };
                break;
        }

        foreach (string alias in aliases)
        {
            if (normalized == alias)
                return true;
        }

        return false;
    }

    public static void Print<T>(MyLinkedList<T> collection)
    {
        if (collection.Count == 0)
        {
            Console.WriteLine("No records found.");
            return;
        }

        int index = 1;

        foreach (T item in collection)
        {
            Console.WriteLine(
                $"\n========== Record #{index} ==========");

            Console.WriteLine(item);

            index++;
        }
    }
}