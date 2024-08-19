using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState {
    public Vector3[] directions = { Vector3.forward, Vector3.left, Vector3.right, Vector3.back };
    public float[] speeds = { 125, 250f, 350f, 500f };
    public float[] durations = { 2f, 1f, 5f, 3f };
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {

    }
    public override void EnterState() {
        Debug.Log($"entered {this.GetType()}");
    }

    public override void ExitState() {
    }

    public override void FrameUpdate() {
    }

    public override void PhysicsUpdate() {
        enemy.IdleMovement(directions[Random.Range(0, directions.Length-1)], speeds[Random.Range(0, speeds.Length-1)], durations[Random.Range(0, durations.Length - 1)]);
    }
}
