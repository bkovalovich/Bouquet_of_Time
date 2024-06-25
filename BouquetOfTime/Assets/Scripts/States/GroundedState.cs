using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundedState : PlayerState
{
    public FloatVariableSO gravityMagnitude;

    public UnityEvent OnAirbourneExit;

    private bool jump;

    private void OnEnable()
    {
        input.OnJump += Jump;
    }

    private void OnDisable()
    {
        input.OnJump -= Jump;
    }

    public override void FrameUpdate()
    {
        CheckGrounded();

        if(jump)
        {
            jump = false;
            rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    public override void PhysicsUpdate()
    {
        Move();
    }

    private void Move()
    {
        float y = rb.velocity.y;
        Vector3 moveDir = new Vector3(input.MoveDirection.x, y, input.MoveDirection.y);
        Vector3 horizontalCameraDir = Camera.main.transform.forward;
        horizontalCameraDir.y = 0;
        horizontalCameraDir.Normalize();
        Quaternion CameraRotation = Quaternion.FromToRotation(Vector3.forward, horizontalCameraDir);

        moveDir = CameraRotation * moveDir;

        rb.velocity = Vector3.MoveTowards(rb.velocity, moveDir * 10, 50 * Time.deltaTime);
    }

    private void Jump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.started)
        {
            jump = true;
        }
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



}
