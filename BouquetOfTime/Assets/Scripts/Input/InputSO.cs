using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Input")]
public class InputSO : ScriptableObject {

    public Action<InputAction.CallbackContext> OnMove;
    public Vector2 MoveDirection;

    public Action<InputAction.CallbackContext> OnSprint;

    public Action<InputAction.CallbackContext> OnJump;

    public Action<InputAction.CallbackContext> OnInteract;
    public Action<InputAction.CallbackContext> OnPrimaryAttack;
    public Action<InputAction.CallbackContext> OnSecondaryAttack;



}
