using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SprintAttackState : PlayerState
{


    [SerializeField] Animator animator;

    [SerializeField] FloatVariableSO gravityMagnitude;

    [SerializeField] EventSO attackFinishedEvent;

    public UnityEvent OnAttackFinishedExit;

    protected virtual void OnEnable()
    {
        animator.CrossFade("SprintAttack", 0.25f);
        attackFinishedEvent.Subscribe(OnAttackFinished);
    }

    private void OnAttackFinished()
    {
        ExitAttackFinished();
    }

    private void OnDisable()
    {
        attackFinishedEvent.Unsubscribe(OnAttackFinished);
    }

    public override void FrameUpdate()
    {
        
    }

    public override void PhysicsUpdate()
    {
        rb.AddForce(Vector3.down * gravityMagnitude, ForceMode.Acceleration);

        rb.velocity *= 0.95f;
    }

    protected virtual void ExitAttackFinished()
    {
        OnAttackFinishedExit?.Invoke();
    }
}
