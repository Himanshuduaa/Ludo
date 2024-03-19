using System.Collections.Generic;
using UnityEngine;

public class CircularList<T>
{
    private List<T> items;
    private int currentIndex = 0;

    public CircularList()
    {
        items = new List<T>();
    }

    public void Add(T item)
    {
        items.Add(item);
    }

    public T GetCurrent()
    {
        if (items.Count == 0)
        {
            Debug.LogError("CircularList is empty");
            return default(T);
        }

        return items[currentIndex];
    }

    public T GetNext()
    {
        if (items.Count == 0)
        {
            Debug.LogError("CircularList is empty");
            return default(T);
        }

        currentIndex = (currentIndex + 1) % items.Count;
        return items[currentIndex];
    }
}
