using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Bouquet
{
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

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (rb.velocity.sqrMagnitude < 0.5f)
            {
                ExitSprint();
            }
        }

        protected override void OnSprint(InputAction.CallbackContext context)
        {
            Debug.Log("Exitting Sprint because the button was released");
            if (context.canceled)
            {
                ExitSprint();
            }
        }



        protected void ExitAttack()
        {
            OnAttackExit?.Invoke();
        }
    }
}
