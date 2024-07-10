using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerState : MonoBehaviour
{
    [SerializeField] protected InputSO input;

    public PlayerInfoSO playerInfo;

    protected Rigidbody rb => playerInfo.rb;

    public abstract void FrameUpdate();
    public abstract void PhysicsUpdate();

    public void EnterState(PlayerState lastState)
    {
        OnEnter?.Invoke(lastState);

    }

    public void ExitState()
    {
        OnExit?.Invoke();
        gameObject.SetActive(false);
    }

    public UnityEvent<PlayerState> OnEnter;
    public UnityEvent OnExit;
}
