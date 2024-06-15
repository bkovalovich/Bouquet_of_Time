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
        
    }

    private void OnAnimatorMove()
    {
        //rb.position += animator.deltaPosition;
    }
}
