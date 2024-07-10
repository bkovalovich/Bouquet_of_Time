using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRootMotion : MonoBehaviour
{

    public Rigidbody rb;
    public Animator animator;
    [SerializeField] PlayerInfoSO playerInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //make this framerate independent
        animator.SetFloat("Velocity", rb.velocity.magnitude);
        animator.SetFloat("VelocityX", rb.velocity.x);
        animator.SetFloat("VelocityY", rb.velocity.y);
        animator.SetFloat("VelocityZ", rb.velocity.z);
        animator.SetFloat("HorizontalVelocity", Vector3.ProjectOnPlane(rb.velocity, playerInfo.Normal).magnitude);

    }

    private void OnAnimatorMove()
    {
        rb.position += animator.deltaPosition;
    }
}
