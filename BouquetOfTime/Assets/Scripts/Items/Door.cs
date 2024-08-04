using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject door;
    public ItemSO silverKey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("trigger enter");
            var inventory = other.GetComponent<PlayerInventory>();
            if (inventory.ContainsItem(silverKey))
            {
                door.SetActive(false);
                inventory.RemoveItem(silverKey);
            }
        }
    }
}
