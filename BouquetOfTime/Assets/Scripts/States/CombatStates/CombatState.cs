using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Bouquet
{
    public class CombatState : PlayerState
    {
        [SerializeField] protected FloatVariableSO gravityMagnitude;

        public Animator animator;
        public CombatState nextState;

        public override void EnterState()
        {
            base.EnterState();
            animator.SetTrigger("DoNextAttack");
        }

        public override void FrameUpdate()
        {
            
        }

        public override void PhysicsUpdate()
        {
            float y = rb.velocity.y;
            rb.velocity *= 0.75f;
            rb.velocity = new Vector3(rb.velocity.x, y, rb.velocity.z);

            rb.AddForce(Vector3.down * gravityMagnitude, ForceMode.Acceleration);
        }

        public void OnAttackCompleted()
        {

        }

        public void OnWindUpFinished()
        {

        }

        public void OnStartHitbox()
        {

        }
    }
}
