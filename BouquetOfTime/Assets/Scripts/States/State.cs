using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bouquet
{
    public abstract class State : MonoBehaviour
    {
        public State ParentState;

        protected double timeEntered;

        public abstract void FrameUpdate();
        public abstract void PhysicsUpdate();

        public virtual void EnterState()
        {
            timeEntered = Time.time;
            OnEnter?.Invoke();
        }

        public virtual void ExitState()
        {
            OnExit?.Invoke();
        }

        public UnityEvent OnEnter;
        public UnityEvent OnExit;
    }
}
