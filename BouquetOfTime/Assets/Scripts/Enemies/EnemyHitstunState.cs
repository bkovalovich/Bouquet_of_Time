using Bouquet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitstunState : EnemyState {
    private float currentTime, maxTime = 0.5f;
    private Vector3 currentKnockback = Vector3.zero;
    public EnemyHitstunState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {

    }
    public override void EnterState() {
        currentTime = 0;
        enemy.rend.material.color = enemy.hitColor;
        Debug.Log("In hitstun");
    }

    public override void ExitState() {
        enemy.rend.material.color = enemy.defaultColor;
        Debug.Log("leaving hitstun");
    }

    public override void FrameUpdate() {
    }

    public override void PhysicsUpdate() {
        if (currentTime < maxTime) {
            currentTime += Time.deltaTime;
            enemy.Knockback();
        } else {
            enemyStateMachine.ChangeState(enemy.enemyIdleState);
        }
    }
}
