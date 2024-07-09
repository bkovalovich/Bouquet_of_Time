using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.ProBuilder;

public class GroundedState : PlayerState
{
    public LayerMask groundMask;

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
            ExitAirbourne();
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
                playerInfo.Normal = hitInfo.normal;
            }
            else
            {
                playerInfo.Normal = Vector3.down;
                SnapToGround();
            }
        }
        else
        {
            ExitAirbourne();
        }
    }

    protected void SnapToGround()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position + -playerInfo.Normal * (playerInfo.currentCollider.height * 0.5f - playerInfo.currentCollider.radius - 0.05f), Vector3.down);

        float raydistance = 2f;

        /*if (!Physics.Raycast(transform.position + -playerInfo.Normal * (playerInfo.currentCollider.height * 0.5f), ray.direction, raydistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            ExitAirbourne();
            return;
        }*/

        if (Physics.SphereCast(ray, playerInfo.currentCollider.radius, out hitInfo, raydistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.cyan, 1);
            //transform.position = transform.position + transform.up * (-Vector3.Dot(transform.up, transform.position) + Vector3.Dot(transform.up, hitInfo.point) + currentCollider.height * 0.5f + BUFFER_DIST);
            transform.position -= transform.up * (hitInfo.distance - 0.05f);
            playerInfo.Normal = hitInfo.normal.normalized;
            //rb.velocity = Vector3.ProjectOnPlane(rb.velocity, Normal).normalized * rb.velocity.magnitude;
            //rb.velocity -= Normal * Vector3.Dot(rb.velocity, Normal);
            //rb.velocity -= GravityDir.normalized * Vector3.Dot(rb.velocity, GravityDir.normalized);
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
