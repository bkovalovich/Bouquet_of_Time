using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider : MonoBehaviour
{
    [SerializeField] InputSO input;



    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input.OnMove?.Invoke(context);
        if (context.performed)
        {
            input.MoveDirection = context.ReadValue<Vector2>();
            
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jumped? " + context.started);
        input.OnJump?.Invoke(context);
    }
}
