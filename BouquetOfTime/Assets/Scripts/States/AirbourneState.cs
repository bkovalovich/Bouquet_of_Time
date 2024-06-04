using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirbourneState : PlayerState
{

    public FloatVariableSO gravityMagnitude;

    public override void FrameUpdate()
    {
        
    }

    public override void PhysicsUpdate()
    {
        rb.AddForce(Vector3.down * gravityMagnitude, ForceMode.Acceleration);
    }
}
