using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject door;
    public ItemSO key;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var inventory = other.GetComponent<PlayerInventory>();
            if (inventory.Contains(key))
            {
                door.SetActive(false);
                inventory.Remove(key, 1);
            }
        }
    }
}
