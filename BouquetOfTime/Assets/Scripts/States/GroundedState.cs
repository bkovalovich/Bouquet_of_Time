using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GroundedState : PlayerState
{
    public float maxSpeed;

    public float acceleration;

    public FloatVariableSO gravityMagnitude;

    public UnityEvent OnAirbourneExit;
    public UnityEvent OnSprintExit;

    private bool jump;

    protected virtual void OnEnable()
    {
        input.OnJump += Jump;
        input.OnSprint += OnSprint;
    }

    protected virtual void OnSprint(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ExitSprint();
        }
    }

    protected void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jump = true;
        }
        else if(context.canceled)
        {
            if(rb.velocity.y > 0)
            {
                rb.velocity *= 0.5f;
            }
        }
    }


    protected virtual void OnDisable()
    {
        input.OnJump -= Jump;
        input.OnSprint -= OnSprint;
    }

    public override void FrameUpdate()
    {
        CheckGrounded();

        DoJump();
    }

    public override void PhysicsUpdate()
    {
        Move();
    }

    protected virtual void DoJump()
    {
        if (jump)
        {
            jump = false;
            rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    protected virtual void Move()
    {
        Vector3 velocity = rb.velocity;
        float y = velocity.y;
        Vector3 moveDir = new Vector3(input.MoveDirection.x, y, input.MoveDirection.y);
        Vector3 horizontalCameraDir = Camera.main.transform.forward;
        horizontalCameraDir.y = 0;
        horizontalCameraDir.Normalize();
        Quaternion CameraRotation = Quaternion.FromToRotation(Vector3.forward, horizontalCameraDir);

        moveDir = CameraRotation * moveDir;

        velocity = Vector3.MoveTowards(velocity, moveDir * maxSpeed, acceleration * Time.deltaTime);

        rb.AddForce((velocity - rb.velocity) / Time.deltaTime);


    }

   

    public void CheckGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1.2f))
        {
            if (hitInfo.normal.y > 0.6f)
            {
                
            }
            else
            {
                ExitAirbourne();
            }
        }
        else
        {
            ExitAirbourne();
        }
    }

    public void ExitAirbourne()
    {
        ExitState();

        OnAirbourneExit?.Invoke();
    }

    public void ExitSprint()
    {
        ExitState();

        OnSprintExit?.Invoke();
    }



}
