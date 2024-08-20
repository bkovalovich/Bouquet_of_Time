using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] float hopVelocity;
    [SerializeField] float damage = 10f; // Amount of damage the slime deals
    private bool inAir; 

    private void Awake()
    {
        stateMachine = new EnemyStateMachine();
        enemyIdleState = new SlimeIdleState(this, stateMachine);
        enemyChaseState = new EnemyChaseState(this, stateMachine);

        rb = GetComponent<Rigidbody>();
    }

    new public void IdleMovement(Vector3 direction, float speed, float duration)
    {
        Debug.Log("slime idle started");

        if (idleEnumeratorRunning == false)
        {
            StopCoroutine(IdleMotion(direction, speed, duration));
            StartCoroutine(IdleMotion(direction, speed, duration));
        }
    }

    IEnumerator IdleMotion(Vector3 direction, float speed, float duration)
    {
        idleEnumeratorRunning = true;
        rb.velocity = direction * speed * Time.deltaTime;
        rb.velocity = new Vector3(rb.velocity.x, Vector3.up.y * hopVelocity * Time.deltaTime, rb.velocity.z);
        yield return new WaitForSeconds(duration);
        idleEnumeratorRunning = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Detected with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Player!");
            PlayerHealthUI playerHealthUI = collision.gameObject.GetComponent<PlayerHealthUI>();
            if (playerHealthUI != null)
            {
                Debug.Log("Applying Damage");
                playerHealthUI.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("PlayerHealthUI component not found on player!");
            }
        }
    }

}
