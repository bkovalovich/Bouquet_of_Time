using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bouquet
{
    public class PlayerCombatStateMachine : StateMachine
    {
        public Animator animator;

        public bool isCancelable;
        public bool isComplete;

        [SerializeField] float inputBufferTime;
        public float InputTime;

        bool attackPressed;
        private void OnEnable()
        {
            ((PlayerStateMachine)ParentState)._input.OnPrimaryAttack += OnPrimaryAttack;
        }

        private void OnPrimaryAttack(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                InputTime = Time.time;
            }
        }

        private void OnDisable()
        {
            ((PlayerStateMachine)ParentState)._input.OnPrimaryAttack -= OnPrimaryAttack;

        }

        public void OnAttackFinished()
        {
            ((PlayerStateMachine)ParentState).TransitionOut();
        }

        public void OnAttackCompleted()
        {
            ((CombatState)CurrentState).OnAttackCompleted();
            isCancelable = true;
            isComplete = true;
        }

        public void OnWindUpFinished()
        {
            ((CombatState)CurrentState).OnWindUpFinished();
            isCancelable = false;
        }

        public void OnStartHitbox()
        {
            ((CombatState)CurrentState).OnStartHitbox();
        }



        public override void EnterState()
        {
            base.EnterState();
            if(((PlayerStateMachine)ParentState).playerInfo.rb.velocity.sqrMagnitude > 9)
            {

            }
            else
            {

            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            if (isComplete && Time.time - InputTime < inputBufferTime)
            {

            }
        }

        public void EnterNextState()
        {
            isComplete = false;
            isCancelable = true;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

        }
    }
}
