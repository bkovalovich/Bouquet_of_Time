using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AnimationController : MonoBehaviour
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

        Vector3 deltaVel = rb.velocity - previousVelocity;
        acceleration = Vector3.Slerp(acceleration, deltaVel / Time.deltaTime, 1 - Mathf.Pow(0.0001f, Time.deltaTime));
        Debug.DrawRay(transform.position + transform.up, acceleration * 5f, Color.green);
        previousVelocity = rb.velocity;
    }

    private void LateUpdate()
    {
        Vector3 localVelocity = transform.worldToLocalMatrix * rb.velocity;
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetFloat("VelocityX", localVelocity.x);
        animator.SetFloat("VelocityY", Mathf.MoveTowards(animator.GetFloat("VelocityY"), localVelocity.y, 80 * Time.deltaTime));
        animator.SetFloat("VelocityZ", localVelocity.z);
        animator.SetFloat("HorizontalVelocity", Vector3.ProjectOnPlane(rb.velocity, playerInfo.Normal).magnitude);
        animator.SetFloat("VDotA", Vector3.Dot(Vector3.ProjectOnPlane(rb.velocity, playerInfo.Normal).normalized, Vector3.ProjectOnPlane(acceleration, playerInfo.Normal).normalized));
        float accelAngle = Vector3.SignedAngle(Vector3.ProjectOnPlane(rb.velocity, transform.up), Vector3.ProjectOnPlane(acceleration, transform.up), transform.up);
        animator.SetFloat("AccelerationAngleDifference", Mathf.Lerp(animator.GetFloat("AccelerationAngleDifference"), accelAngle * acceleration.magnitude * 0.1f, 1 - Mathf.Pow(0.0001f, Time.deltaTime)));
        animator.SetBool("Grounded", playerInfo.Grounded);
        
    }

    public void Setup(PlayerInfoSO info, InputSO input)
    {
        playerInfo = info;
    }

    

    private void OnAnimatorMove()
    {
        rb.position += animator.deltaPosition;
        transform.parent.rotation = animator.deltaRotation * transform.parent.rotation;
    }
}
