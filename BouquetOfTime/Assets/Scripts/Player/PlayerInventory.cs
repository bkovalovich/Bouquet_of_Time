using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Dictionary<ItemSO, int> inventory;

    public void OnEnable()
    {
        inventory = new Dictionary<ItemSO, int>();
    }

    public void Add(ItemSO item, int count)
    {
        if (inventory.TryGetValue(item, out int itemCount))
        {
            inventory[item] += count;
        }
        else
        {
            inventory.Add(item, 1);
        }
    }

    public void Remove(ItemSO item, int count)
    {
        if (inventory.TryGetValue(item, out int itemCount))
        {
            if (itemCount > count)
            {
                inventory[item] -= count;
            }
            else
            {
                inventory.Remove(item);
            }
        }
    }

    public void RemoveAll(ItemSO item)
    {
        if (inventory.TryGetValue(item, out int itemCount))
        {
            inventory.Remove(item);
        }
    }

    public bool Contains(ItemSO item)
    {
        return inventory.ContainsKey(item);
    }

    public int GetCount(ItemSO item)
    {
        if (inventory.TryGetValue(item, out int itemCount))
        {
            return itemCount;
        }
        else
        {
            return 0;
        }
    }
}
