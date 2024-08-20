using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeChaseState : EnemyState {
    private float currentTime, maxTime;
    protected Slime slime;
    public SlimeChaseState(Slime slime, EnemyStateMachine enemyStateMachine) : base(slime, enemyStateMachine) {
        this.slime = slime;
        maxTime = slime.maxTime;
    }
    public override void EnterState() {
        currentTime = maxTime;
    }

    public override void ExitState() {
    }

    public override void FrameUpdate() {
    }

    public override void PhysicsUpdate() {
        if (currentTime < maxTime) {
            currentTime += Time.deltaTime;
        } else {
            currentTime = 0;
            slime.ChasePlayer();
        }
    }

}
