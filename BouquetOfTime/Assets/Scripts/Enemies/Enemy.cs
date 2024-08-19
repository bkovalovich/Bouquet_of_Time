using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region State Machine
    public EnemyStateMachine stateMachine;
    public EnemyState enemyIdleState, enemyChaseState;
    #endregion
    #region Component Refs
    protected Rigidbody rb;
    #endregion
    public GameObject playerObj;
    protected bool idleEnumeratorRunning = false;

    private void Awake() {
        stateMachine = new EnemyStateMachine();
        enemyIdleState = new EnemyIdleState(this, stateMachine);
        enemyChaseState = new EnemyChaseState(this, stateMachine);

        rb = GetComponent<Rigidbody>();
    }
    private void Start() {
        stateMachine.Initialize(enemyIdleState);
    }

    public void OnPlayerEnterRange() {
        Debug.Log("player entered");
        stateMachine.ChangeState(enemyChaseState);
    }
    public void OnPlayerLeaveRange() {
        stateMachine.ChangeState(enemyIdleState);
    }
    public void IdleMovement(Vector3 direction, float speed, float duration) {
        if(idleEnumeratorRunning == false) {
            StopCoroutine(IdleMotion(direction, speed, duration));
            StartCoroutine(IdleMotion(direction, speed, duration));
        }
    }
    public void ChasePlayer() {
        transform.position = Vector3.MoveTowards(transform.position, playerObj.transform.position, 1 * Time.deltaTime);
    }
    IEnumerator IdleMotion(Vector3 direction, float speed, float duration) {
        idleEnumeratorRunning = true;
        rb.velocity = direction * speed * Time.deltaTime;
        yield return new WaitForSeconds(duration);
        idleEnumeratorRunning = false;
    }

    private void Update() {
        stateMachine.CurrentState.FrameUpdate();
    }
    private void FixedUpdate() {
        stateMachine.CurrentState.PhysicsUpdate();
    }
}
