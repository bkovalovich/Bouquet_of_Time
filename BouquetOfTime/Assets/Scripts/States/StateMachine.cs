using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] PlayerState defaultState;

    [SerializeField] PlayerState currentState;

    private void OnEnable()
    {
        /*Rigidbody rb = GetComponentInParent<Rigidbody>();
        foreach (PlayerState state in GetComponentsInChildren<PlayerState>())
        {
            state.rb = rb;
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {
        TransitionTo(defaultState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(); 
    }

    public void TransitionTo(PlayerState state)
    {
        state.EnterState(currentState);
        currentState = state;
    }
}
