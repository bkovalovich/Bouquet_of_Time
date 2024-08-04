using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeliveryArea : MonoBehaviour
{
    public ItemSO key;
    public GameObject messageUI;
    public string message;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var inventory = other.GetComponent<PlayerInventory>();
            if (inventory.ContainsItem(key))
            {
                messageUI.SetActive(true);
                messageUI.GetComponent<TextMeshProUGUI>().text = message;
                inventory.RemoveItem(key);
            }
        }
    }
}
