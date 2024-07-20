using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Bouquet
{
    public class DodgeState : PlayerState
    {
        [SerializeField] Animator animator;

        [SerializeField] float dodgeTime;

        public override void EnterState()
        {
            base.EnterState();

        }

        public override void FrameUpdate()
        {
            
        }

        public override void PhysicsUpdate()
        {
            
        }
    }
}
