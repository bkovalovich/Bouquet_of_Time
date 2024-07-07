using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SprintAttackState : PlayerState
{

    public UnityEvent OnAttackFinishedExit;

    public override void FrameUpdate()
    {
        
    }

    public override void PhysicsUpdate()
    {
        rb.velocity /= 1.1f;
    }

    protected virtual void ExitAttackFinished()
    {
        ExitState();

        OnAttackFinishedExit?.Invoke();
    }
}
