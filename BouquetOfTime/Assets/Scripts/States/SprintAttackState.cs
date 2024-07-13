using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SprintAttackState : PlayerState
{

    public UnityEvent OnAttackFinishedExit;

    [SerializeField] Animator animator;

    protected virtual void OnEnable()
    {
        animator.CrossFade("SprintAttack", 0.25f);
    }

    public override void FrameUpdate()
    {
        
    }

    public override void PhysicsUpdate()
    {
        rb.velocity *= 0.95f;
    }

    protected virtual void ExitAttackFinished()
    {
        OnAttackFinishedExit?.Invoke();
    }
}
