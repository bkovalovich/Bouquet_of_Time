using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseThings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Physics.queriesHitTriggers = false;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
