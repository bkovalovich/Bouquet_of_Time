using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] PlayerInfoSO playerInfo;

    [SerializeField] PlayerState defaultState;

    [SerializeField] PlayerState currentState;

    private void OnEnable()
    {
        if(!playerInfo)
        {
            playerInfo = new PlayerInfoSO();
        }
        playerInfo.rb = GetComponentInParent<Rigidbody>();
        Debug.Log(playerInfo.rb);
        foreach (PlayerState state in GetComponentsInChildren<PlayerState>())
        {
            state.playerInfo = playerInfo;
        }
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
