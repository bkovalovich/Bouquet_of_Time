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
        public InputSO _input;
        public PlayerInfoSO playerInfo;

        public bool isCancelable;
        public bool isComplete;

        [SerializeField] float inputBufferTime;
        public float InputTime;

        private bool inputUsed;

        public bool AttackInput => Time.time - InputTime < inputBufferTime && !inputUsed;

        [SerializeField] CombatState sprintAttackState;
        [SerializeField] CombatState firstAttackState;

        bool attackPressed;
        private void OnEnable()
        {
            ((PlayerStateMachine)ParentState)._input.OnPrimaryAttack += OnPrimaryAttack;
            foreach(CombatState state in gameObject.GetComponentsInChildren<CombatState>(true))
            {
                state.ParentState = this;
                state.playerInfo = playerInfo;
                state.input = _input;
                state.animator = animator;
                state.gameObject.SetActive(false);
            }
        }

        private void OnPrimaryAttack(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                InputTime = Time.time;
                inputUsed = false;
            }
        }

        private void OnDisable()
        {
            ((PlayerStateMachine)ParentState)._input.OnPrimaryAttack -= OnPrimaryAttack;

        }

        public void OnAttackFinished()
        {
            if (isComplete)
            {
                ((PlayerStateMachine)ParentState).TransitionOut();
            }
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

            TransitionTo(null);

            isComplete = false;

            if (((PlayerStateMachine)ParentState).playerInfo.rb.velocity.sqrMagnitude > 9 * 9)
            {
                TransitionTo(sprintAttackState);
            }
            else
            {
                TransitionTo(firstAttackState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            if (isComplete && AttackInput)
            {
                inputUsed = true;
                EnterNextState();
            }
        }

        public void EnterNextState()
        {
            CombatState nextState = ((CombatState)CurrentState).nextState;
            if (nextState == null)
            {
                return;
            }
            isComplete = false;
            isCancelable = true;
            TransitionTo(nextState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

        }
    }
}
