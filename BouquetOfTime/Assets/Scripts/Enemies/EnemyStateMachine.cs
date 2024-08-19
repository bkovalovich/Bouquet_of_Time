using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine {
    public EnemyState currentState;
    public EnemyState CurrentState { get; set; }

    public void Initialize(EnemyState startingState) {
        CurrentState = startingState;
        CurrentState.EnterState();
    }
    public void ChangeState(EnemyState newState) {
        if (newState == null) { return; }
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
