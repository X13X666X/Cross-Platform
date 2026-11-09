using System;
using System.Collections;
using System.Collections.Generic;

namespace ShopCatalog14;

public class MyLinkedList<T> : IEnumerable<T>
{
    private class Node
    {
        public T Data;
        public Node Next;

        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    private Node head;
    private Node tail;

    public int Count { get; private set; }

    public MyLinkedList()
    {
        head = null;
        tail = null;
        Count = 0;
    }

    public void Add(T item)
    {
        Node newNode = new Node(item);

        if (head == null)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            tail.Next = newNode;
            tail = newNode;
        }

        Count++;
    }

    public T Get(int index)
    {
        CheckIndex(index);

        Node current = head;

        for (int i = 0; i < index; i++)
            current = current.Next;

        return current.Data;
    }

    public void Update(int index, T item)
    {
        CheckIndex(index);

        Node current = head;

        for (int i = 0; i < index; i++)
            current = current.Next;

        current.Data = item;
    }

    public void RemoveAt(int index)
    {
        CheckIndex(index);

        if (index == 0)
        {
            head = head.Next;
            Count--;

            if (Count == 0)
                tail = null;

            return;
        }

        Node current = head;

        for (int i = 0; i < index - 1; i++)
            current = current.Next;

        current.Next = current.Next.Next;

        if (index == Count - 1)
            tail = current;

        Count--;
    }

    public void Clear()
    {
        head = null;
        tail = null;
        Count = 0;
    }

    public T[] ToArray()
    {
        T[] result = new T[Count];

        int index = 0;

        foreach (T item in this)
        {
            result[index] = item;
            index++;
        }

        return result;
    }

    private void CheckIndex(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));
    }

    public IEnumerator<T> GetEnumerator()
    {
        Node current = head;

        while (current != null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}