using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy {
    [SerializeField] public float hopVelocity, chaseVelocity, maxTime;
    private bool inAir; 
    private Vector3 chaseDirection = Vector3.zero;
    private AudioSource plop; 

    private void Awake() {
        stateMachine = new EnemyStateMachine();
        enemyIdleState = new SlimeIdleState(this, stateMachine);
        enemyChaseState = new SlimeChaseState(this, stateMachine);
        enemyHitState = new EnemyHitstunState(this, stateMachine);  

        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        plop = GetComponent<AudioSource>();
        defaultColor = rend.material.color;
    }
    new public void IdleMovement(Vector3 direction, float speed, float duration) {
        if (idleEnumeratorRunning == false) {
            StopCoroutine(IdleMotion(direction, speed, duration));
            StartCoroutine(IdleMotion(direction, speed, duration));
        }
    }

    new public void ChasePlayer() {
        Debug.Log("chaseplayer called");
        plop.PlayOneShot(plop.clip);
        chaseDirection = (playerObj.transform.position - transform.position).normalized * chaseVelocity;
        rb.velocity = new Vector3(chaseDirection.x, Vector3.up.y * hopVelocity * Time.deltaTime, chaseDirection.z);
    }

    IEnumerator IdleMotion(Vector3 direction, float speed, float duration) {
        idleEnumeratorRunning = true;
        plop.PlayOneShot(plop.clip);
        rb.velocity = direction * speed * Time.deltaTime;
        rb.velocity = new Vector3(rb.velocity.x, /*Vector3.up.y * hopVelocity * Time.deltaTime*/0, rb.velocity.z);
        yield return new WaitForSeconds(duration);
        idleEnumeratorRunning = false;
    }

    public void OnPlayerEnterRange() {
        //Debug.Log("player entered");
        stateMachine.ChangeState(enemyChaseState);
    }

    public void OnPlayerLeaveRange() {
        stateMachine.ChangeState(enemyIdleState);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "PHitbox") {
            currentKnockback = other.transform.forward * Random.Range(8, 12);
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
