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

    public void AddItem(ItemSO item)
    {
        if (inventory.TryGetValue(item, out int itemCount))
        {
            inventory[item] = itemCount++;
        }
        else
        {
            inventory.Add(item, 1);
        }
    }

    public void RemoveItem(ItemSO item)
    {
        if (inventory.TryGetValue(item, out int itemCount))
        {
            if (itemCount > 1)
            {
                inventory[item] = itemCount--;
            }
            else
            {
                inventory.Remove(item);
            }
        }
    }

    public bool ContainsItem(ItemSO item)
    {
        return inventory.ContainsKey(item);
    }
}
