using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AirbourneState : PlayerState
{

    public FloatVariableSO gravityMagnitude;

    public UnityEvent OnGroundedExit;

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
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, 1.5f))
        {
            if(hitInfo.normal.y > 0.6f)
            {
                ExitGrounded();
            }
        }
    }

    public void ExitGrounded()
    {
        ExitState();

        OnGroundedExit?.Invoke();
    }
}
