using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState {
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {

    }
    public override void EnterState() {
        Debug.Log($"entered {this.GetType()}");
    }

    public override void ExitState() {
    }

    public override void FrameUpdate() {
    }

    public override void PhysicsUpdate() {
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.playerObj.transform.position, 1 * Time.deltaTime);
    }
}
