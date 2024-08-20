using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy {
    [SerializeField] float hopVelocity;
    private bool inAir; 

    private void Awake() {
        stateMachine = new EnemyStateMachine();
        enemyIdleState = new SlimeIdleState(this, stateMachine);
        enemyChaseState = new EnemyChaseState(this, stateMachine);
        enemyHitState = new EnemyHitstunState(this, stateMachine);  

        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        defaultColor = rend.material.color;
    }
    new public void IdleMovement(Vector3 direction, float speed, float duration) {
        if (idleEnumeratorRunning == false) {
            StopCoroutine(IdleMotion(direction, speed, duration));
            StartCoroutine(IdleMotion(direction, speed, duration));
        }
    }

    IEnumerator IdleMotion(Vector3 direction, float speed, float duration) {
        idleEnumeratorRunning = true;
        rb.velocity = direction * speed * Time.deltaTime;
        rb.velocity = new Vector3(rb.velocity.x, Vector3.up.y * hopVelocity * Time.deltaTime, rb.velocity.z);
        yield return new WaitForSeconds(duration);
        idleEnumeratorRunning = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "PHitbox") {
            currentKnockback = (other.gameObject.transform.position - gameObject.transform.position).normalized * -15;
            stateMachine.ChangeState(enemyHitState);
        }
    }

    private void Update() {
        stateMachine.CurrentState.FrameUpdate();
    }
    private void FixedUpdate() {
        stateMachine.CurrentState.PhysicsUpdate();
    }
}
