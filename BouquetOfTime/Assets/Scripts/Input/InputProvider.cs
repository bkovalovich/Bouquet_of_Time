using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider : MonoBehaviour
{
    [SerializeField] InputSO input;

    public void OnMove(InputAction.CallbackContext context)
    {
        input.OnMove?.Invoke(context);
        if (context.performed)
        {
            Debug.Log($"MoveValue: {context.ReadValue<Vector2>()}");
            input.MoveDirection = context.ReadValue<Vector2>();
            
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jumped? " + context.started);
        input.OnJump?.Invoke(context);
    }
}
