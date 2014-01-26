using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class Inventory
{
    /// <summary>
    /// Вместительность инвентаря
    /// </summary>
    public uint Capacity;

    public List<Item> Items;

    protected Inventory(uint capacity)
    {
        Capacity = capacity;
        Items = new List<Item>((int)capacity);
    }

    public static Inventory InitWithCapacity(uint capacity)
    {
        var inventory = new Inventory(capacity);

        inventory.Items.AddRange(Enumerable.Repeat<Item>(null, (int) capacity));

        return inventory;
    }
}
