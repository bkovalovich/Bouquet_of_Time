using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRootMotion : MonoBehaviour
{

    public Rigidbody rb;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //make this framerate independent
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), rb.velocity.magnitude * 0.07f, 1 - Mathf.Pow(0.005f, Time.deltaTime)));
    }

    private void OnAnimatorMove()
    {
        rb.position += animator.deltaPosition;
    }
}
