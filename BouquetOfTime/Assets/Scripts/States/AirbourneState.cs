using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AirbourneState : PlayerState
{
    [SerializeField] protected float maxSpeed;

    protected float targetSpeed;

    protected float enterSpeed;

    public float acceleration;

    public FloatVariableSO gravityMagnitude;

    public UnityEvent OnGroundedExit;

    public override void EnterState(PlayerState lastState)
    {
        base.EnterState(lastState);

        playerInfo.Grounded = false;
        playerInfo.Normal = rb.transform.up;
        enterSpeed = (rb.velocity -  playerInfo.Normal * Vector3.Dot(playerInfo.Normal, rb.velocity)).magnitude;
    }

    public override void FrameUpdate()
    {
        CheckGrounded();
    }

    public override void PhysicsUpdate()
    {
        rb.AddForce(Vector3.down * gravityMagnitude, ForceMode.Acceleration);

        Move();
    }

    public void CheckGrounded()
    {
        Ray ray = new Ray(rb.position - rb.transform.up, Vector3.down);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, 0.1f))
        {
            if(hitInfo.normal.y > 0.6f && rb.velocity.y < 0)
            {
                ExitGrounded();
            }
        }
    }

    protected virtual void Move()
    {
        
        Vector3 velocity = rb.velocity;
        Vector3 horizontalVelocity = Vector3.ProjectOnPlane(velocity, playerInfo.Normal);
        targetSpeed = horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed? enterSpeed : maxSpeed;
        Vector3 y = Vector3.Dot(velocity, playerInfo.Normal) * playerInfo.Normal;
        Vector3 moveDir = new Vector3(input.MoveDirection.x, 0, input.MoveDirection.y);
        Vector3 horizontalCameraDir = Camera.main.transform.forward;
        horizontalCameraDir.y = 0;
        horizontalCameraDir.Normalize();
        Quaternion CameraRotation = Quaternion.FromToRotation(Vector3.forward, horizontalCameraDir);

        moveDir = CameraRotation * moveDir;

        velocity = Vector3.MoveTowards(velocity, Quaternion.FromToRotation(Vector3.up, playerInfo.Normal) * moveDir * targetSpeed + y, acceleration * Time.deltaTime);

        rb.AddForce((velocity - rb.velocity) / Time.deltaTime);

    }

    public void ExitGrounded()
    {
        OnGroundedExit?.Invoke();
    }
}
