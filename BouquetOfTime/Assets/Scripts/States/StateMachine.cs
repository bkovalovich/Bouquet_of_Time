using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bouquet
{
    public class StateMachine : State
    {
        protected State _currentState;
        public virtual State CurrentState => _currentState;

        public State DefaultState;

        public override void FrameUpdate()
        {
            if(CurrentState == null)
            {
                _currentState = DefaultState;
            } 
            _currentState.FrameUpdate();
        }

        public override void PhysicsUpdate()
        {
            if (CurrentState == null)
            {
                _currentState = DefaultState;
            }
            _currentState.PhysicsUpdate();
        }

        public void TransitionTo(State state)
        {
            

            if (state == CurrentState) { return; }

            if (_currentState)
            {
                _currentState.ExitState();
                _currentState.gameObject.SetActive(false);
            }

            if (state == null)
            {
                _currentState = null;
                return;
            }

            state.gameObject.SetActive(true);
            state.EnterState();
            _currentState = state;
        }
    }
}
