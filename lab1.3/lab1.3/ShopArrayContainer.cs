using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ShopCatalog;

public class ShopArrayContainer<T> : IEnumerable<T>
{
    private T[] items;
    private int count;

    public int Count => count;

    public ShopArrayContainer()
    {
        items = new T[4];
        count = 0;
    }

    [JsonInclude]
    public T[] Items
    {
        get
        {
            return ToArray();
        }
        private set
        {
            items = value ?? Array.Empty<T>();
            count = items.Length;

            if (items.Length == 0)
                items = new T[4];
        }
    }

    public void Add(T item)
    {
        if (count == items.Length)
        {
            int newCapacity = items.Length == 0 ? 4 : items.Length * 2;
            Array.Resize(ref items, newCapacity);
        }

        items[count] = item;
        count++;
    }

    public T Get(int index)
    {
        CheckIndex(index);

        return items[index];
    }

    public void Update(int index, T item)
    {
        CheckIndex(index);

        items[index] = item;
    }

    public void RemoveAt(int index)
    {
        CheckIndex(index);

        for (int i = index; i < count - 1; i++)
        {
            items[i] = items[i + 1];
        }

        count--;
        items[count] = default;

        if (count == 0)
        {
            items = new T[4];
        }
    }

    public void Clear()
    {
        items = new T[4];
        count = 0;
    }

    public T[] ToArray()
    {
        T[] result = new T[count];

        Array.Copy(items, result, count);

        return result;
    }

    private void CheckIndex(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameof(index));
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < count; i++)
        {
            yield return items[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}