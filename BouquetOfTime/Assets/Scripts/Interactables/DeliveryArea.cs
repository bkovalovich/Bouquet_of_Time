using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeliveryArea : MonoBehaviour
{
    public ItemSO sand;
    [SerializeField] FloatVariableSO currentTime;
    public float sandValue;

    public ItemSO key;
    public GameObject messageUI;
    public string message;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var inventory = other.GetComponent<PlayerInventory>();
            if (inventory.Contains(key))
            {
                messageUI.SetActive(true);
                messageUI.GetComponent<TextMeshProUGUI>().text = message;
                inventory.Remove(key, 1);
            }

            //automatically deposits all sand; we might want to change this in the future
            if (inventory.Contains(sand))
            {
                currentTime.Value += sandValue * inventory.GetCount(sand);
                inventory.RemoveAll(sand);
            }
        }
    }
}
