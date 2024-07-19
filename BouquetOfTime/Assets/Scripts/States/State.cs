using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bouquet
{
    public abstract class State : MonoBehaviour
    {
        public abstract void FrameUpdate();
        public abstract void PhysicsUpdate();

        public virtual void EnterState()
        {
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
