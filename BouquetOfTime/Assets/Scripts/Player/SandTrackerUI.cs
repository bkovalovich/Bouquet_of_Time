using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SandTrackerUI : MonoBehaviour
{

    [SerializeField] PlayerInventory inventory;
    [SerializeField] TextMeshProUGUI textMesh;

    [SerializeField] ItemSO itemToTrack;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = "" + inventory.GetCount(itemToTrack);
    }
}
