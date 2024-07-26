using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TestRootMotion : MonoBehaviour
{

    public Rigidbody rb;
    public Animator animator;
    [SerializeField] PlayerInfoSO playerInfo;

    private Vector3 previousVelocity;
    private Vector3 acceleration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //make this framerate independent
        /*animator.SetFloat("Velocity", rb.velocity.magnitude);
        animator.SetFloat("VelocityX", rb.velocity.x);
        animator.SetFloat("VelocityY", rb.velocity.y);
        animator.SetFloat("VelocityZ", rb.velocity.z);
        animator.SetFloat("HorizontalVelocity", Vector3.ProjectOnPlane(rb.velocity, playerInfo.Normal).magnitude);
        animator.SetBool("Grounded", playerInfo.Grounded);*/

        acceleration = Vector3.Lerp(acceleration, (rb.velocity - previousVelocity) / Time.deltaTime, 1 - Mathf.Pow(0.01f, Time.deltaTime));
        previousVelocity = rb.velocity;
    }

    private void LateUpdate()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetFloat("VelocityX", rb.velocity.x);
        animator.SetFloat("VelocityY", Mathf.MoveTowards(animator.GetFloat("VelocityY"), rb.velocity.y, 80 * Time.deltaTime));
        animator.SetFloat("VelocityZ", rb.velocity.z);
        animator.SetFloat("HorizontalVelocity", Vector3.ProjectOnPlane(rb.velocity, playerInfo.Normal).magnitude);
        animator.SetFloat("VDotA", Vector3.Dot(Vector3.ProjectOnPlane(rb.velocity, playerInfo.Normal).normalized, Vector3.ProjectOnPlane(acceleration, playerInfo.Normal).normalized));
        animator.SetBool("Grounded", playerInfo.Grounded);

        
    }

    

    private void OnAnimatorMove()
    {
        rb.position += animator.deltaPosition;
        transform.parent.rotation = animator.deltaRotation * transform.parent.rotation;
    }
}
