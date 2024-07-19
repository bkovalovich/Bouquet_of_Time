using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bouquet
{
    public class RootMotionState : PlayerState
    {
        [SerializeField] Animator animator;

        public override void FrameUpdate()
        {

        }

        public override void PhysicsUpdate()
        {
            rb.transform.position += animator.deltaPosition;
            //rb.rotation = animator.deltaRotation * rb.rotation;
        }
    }
}
