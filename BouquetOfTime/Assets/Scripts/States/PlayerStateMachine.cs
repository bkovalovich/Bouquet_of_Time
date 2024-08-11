using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bouquet
{
    public class PlayerStateMachine : StateMachine
    {
        public InputSO _input;
        public PlayerInfoSO playerInfo;
        [SerializeField] Rigidbody rb;
        [SerializeField] LockOnCamera lockOnCamera;

        [Header("States")]
        [SerializeField] AirbourneState airbourneState;
        [SerializeField] GroundedState groundedState;
        [SerializeField] LockedOnGroundedState lockedOnGroundedState;
        [SerializeField] SprintState sprintState;
        [SerializeField] SprintAttackState sprintAttackState;
        [SerializeField] DodgeState dodgeState;

        private bool trySprint;
        private bool tryAttack;
        private bool tryDodge;

        public void Setup(PlayerInfoSO info, InputSO input)
        {
            playerInfo = info;
            _input = input;
            InitializeStates();
        }

        public void InitializeStates()
        {
            if (!playerInfo)
            {
                playerInfo = new PlayerInfoSO();
            }
            if (rb == null)
            {
                rb = GetComponentInParent<Rigidbody>();
            }
            playerInfo.rb = rb;

            foreach (PlayerState state in GetComponentsInChildren<PlayerState>(true))
            {
                state.ParentState = this;
                state.playerInfo = playerInfo;
                state.input = _input;
                state.gameObject.SetActive(false);
            }
        }

        #region enable/disable
        private void OnEnable()
        {
            InitializeStates();
            _input.OnSprint += OnSprint;
            _input.OnPrimaryAttack += OnAttack;
            _input.OnDodge += OnDodge;
        }

        private void OnDodge(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                tryDodge = true;
            }
            if (context.canceled)
            {
                tryDodge = false;
            }
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started) 
            {
                tryAttack = true;
            }
            if (context.canceled)
            {
                tryAttack = false;
            }
        }

        private void OnSprint(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                trySprint = true;
            }
            if(context.canceled)
            {
                trySprint= false;
            }
        }

        private void OnDisable()
        {
            _input.OnSprint -= OnSprint;
            _input.OnPrimaryAttack -= OnAttack;

        }
        #endregion

        void Start()
        {
            TransitionTo(DefaultState);
        }

        void Update()
        {
            _currentState.FrameUpdate();

            ChangeStates();
        }

        private void FixedUpdate()
        {
            _currentState.PhysicsUpdate();
        }

        public void TransitionTo(State state)
        {
            if(state == CurrentState) { return; }

            if (_currentState)
            {
                _currentState.ExitState();
                _currentState.gameObject.SetActive(false);
            }
            state.gameObject.SetActive(true);
            state.EnterState();
            _currentState = state;
        }

        public void TransitionOut()
        {
            TransitionTo(DefaultState);
            ChangeStates();
        }

        public void ChangeStates()
        {
            if(!playerInfo.Grounded)
            {
                TransitionTo(airbourneState);
            }

            if(playerInfo.Grounded && CurrentState == airbourneState) 
            {
                TransitionTo(groundedState);
            }

            if(!trySprint && CurrentState == sprintState)
            {
                TransitionTo(groundedState);
            }

            if(CurrentState == groundedState && lockOnCamera.locked)
            {
                TransitionTo(lockedOnGroundedState);
            }
            if(CurrentState == lockedOnGroundedState && !lockOnCamera.locked)
            {
                TransitionTo(groundedState);
            }

            if(trySprint && typeof(GroundedState).IsAssignableFrom(CurrentState.GetType()))
            {
                TransitionTo(sprintState);
            }

            if(tryAttack && CurrentState == sprintState && rb.velocity.sqrMagnitude > 2.5f)
            {
                tryAttack = false;
                TransitionTo(sprintAttackState);
            }

            if(tryDodge && (CurrentState == groundedState || CurrentState == sprintState || CurrentState == lockedOnGroundedState))
            {
                tryDodge = false;
                TransitionTo(dodgeState);
            }
        }
    }
}
