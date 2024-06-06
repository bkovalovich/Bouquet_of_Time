using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerState : MonoBehaviour
{
    [SerializeField] protected InputSO input;

    public Rigidbody rb;

    public abstract void FrameUpdate();
    public abstract void PhysicsUpdate();

    public void EnterState(PlayerState lastState)
    {
        OnEnter?.Invoke(lastState);

    }

    public void ExitState()
    {
        OnExit?.Invoke();
    }

    public UnityEvent<PlayerState> OnEnter;
    public UnityEvent OnExit;
}
