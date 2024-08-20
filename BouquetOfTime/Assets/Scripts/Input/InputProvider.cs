using Bouquet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider : MonoBehaviour
{
    [SerializeField] InputSO input;

    [SerializeField] PlayerStateMachine playerStateMachine;

    private void OnEnable()
    {
        input = ScriptableObject.CreateInstance<InputSO>();
        if (playerStateMachine == null)
        {
            playerStateMachine = GetComponentInChildren<PlayerStateMachine>(true);
        }
        playerStateMachine._input = input;
        playerStateMachine.playerInfo = ScriptableObject.CreateInstance<PlayerInfoSO>();
        playerStateMachine.gameObject.SetActive(false);
        playerStateMachine.gameObject.SetActive(true);

        foreach (InputActionMap map in GetComponent<PlayerInput>().actions.actionMaps)
        {
            map.Enable();
        }
    }

    private void OnDisable()
    {
        
    }

    public void Setup(PlayerInfoSO info, InputSO input)
    {
        this.input = input;
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
        //Debug.Log("Jumped? " + context.started);
        input.OnJump?.Invoke(context);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //Debug.Log("Sprint? " + context.started);
        input.OnSprint?.Invoke(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //Debug.Log("Interact? " + context.started);
        input.OnInteract?.Invoke(context);
    }

    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        //Debug.Log("PrimaryAttack? " + context.started);
        input.OnPrimaryAttack?.Invoke(context);
    }

    public void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        //Debug.Log("SecondaryAttack? " + context.started);
        input.OnSecondaryAttack?.Invoke(context);
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        input.OnDodge?.Invoke(context);

    }

    public void OnLockOn(InputAction.CallbackContext context)
    {
        input.OnLock?.Invoke(context);
    }
}
