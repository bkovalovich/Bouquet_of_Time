using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AirbourneState : PlayerState
{

    public FloatVariableSO gravityMagnitude;

    public UnityEvent OnGroundedExit;

    public override void EnterState(PlayerState lastState)
    {
        base.EnterState(lastState);

        playerInfo.Grounded = false;
    }

    public override void FrameUpdate()
    {
        CheckGrounded();
    }

    public override void PhysicsUpdate()
    {
        rb.AddForce(Vector3.down * gravityMagnitude, ForceMode.Acceleration);
    }

    public void CheckGrounded()
    {
        Ray ray = new Ray(rb.position - rb.transform.up, Vector3.down);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, 0.08f))
        {
            if(hitInfo.normal.y > 0.6f && rb.velocity.y < 0)
            {
                ExitGrounded();
            }
        }
    }

    public void ExitGrounded()
    {
        OnGroundedExit?.Invoke();
    }
}
