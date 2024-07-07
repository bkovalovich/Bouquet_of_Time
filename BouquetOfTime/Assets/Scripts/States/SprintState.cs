using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class SprintState : GroundedState
{
    public UnityEvent OnAttackExit;

    protected override void OnEnable()
    {
        base.OnEnable();

        input.OnPrimaryAttack += OnAttack;
    }

    protected void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ExitAttack();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        input.OnPrimaryAttack -= OnAttack;
    }

    protected override void OnSprint(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            ExitSprint();
        }
    }

    

    protected void ExitAttack()
    {
        ExitState();

        OnAttackExit?.Invoke();
    }
}
