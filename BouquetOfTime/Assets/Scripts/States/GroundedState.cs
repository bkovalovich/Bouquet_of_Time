using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.ProBuilder;

namespace Bouquet
{
    public class GroundedState : PlayerState
    {
        public LayerMask groundMask;

        public float maxSpeed;

        public float acceleration;

        [SerializeField] protected float JumpHeight = 3;

        protected float jumpForce;

        public FloatVariableSO gravityMagnitude;

        [SerializeField] CapsuleCollider capsuleCollider;

        private bool jump;

        public override void EnterState()
        {
            base.EnterState();
        }

        protected virtual void OnEnable()
        {
            input.OnJump += Jump;

            jumpForce = MathF.Sqrt(2 * gravityMagnitude.Value * JumpHeight);
        }


        protected void Jump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                jump = true;
            }
        }


        protected virtual void OnDisable()
        {
            input.OnJump -= Jump;
        }

        public override void FrameUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
            Move();
            SnapToGround();
            DoJump();
        }

        protected virtual void DoJump()
        {
            if (jump)
            {
                jump = false;

                rb.AddForce(rb.transform.up * jumpForce, ForceMode.Impulse);
                ExitAirbourne();
            }
        }

        protected virtual void Move()
        {
            Vector3 velocity = rb.velocity;
            Vector3 y = Vector3.Dot(velocity, playerInfo.Normal) * playerInfo.Normal;
            Vector3 moveDir = new Vector3(input.MoveDirection.x, 0, input.MoveDirection.y);
            Vector3 horizontalCameraDir = Camera.main.transform.forward;
            horizontalCameraDir.y = 0;
            horizontalCameraDir.Normalize();
            Quaternion CameraRotation = Quaternion.FromToRotation(Vector3.forward, horizontalCameraDir);

            moveDir = CameraRotation * moveDir;

            velocity = Vector3.MoveTowards(velocity, Quaternion.FromToRotation(Vector3.up, playerInfo.Normal) * moveDir * maxSpeed + y, acceleration * Time.deltaTime);

            rb.AddForce((velocity - rb.velocity) / Time.deltaTime);


        }



        public void CheckGrounded()
        {
            Ray ray = new Ray(rb.position + playerInfo.Normal * 0.05f, Vector3.down);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 1.2f))
            {
                if (hitInfo.normal.y > 0.5f)
                {
                    playerInfo.Normal = hitInfo.normal;
                }
                else
                {
                    playerInfo.Normal = Vector3.up;
                    playerInfo.Grounded = false;
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
            Ray ray = new Ray(rb.transform.position - rb.transform.up * (capsuleCollider.height * 0.5f - capsuleCollider.radius - 0.05f), -rb.transform.up);

            float raydistance = 1f;

            /*if (!Physics.Raycast(transform.position + -playerInfo.Normal * (playerInfo.currentCollider.height * 0.5f), ray.direction, raydistance, groundMask, QueryTriggerInteraction.Ignore))
            {
                ExitAirbourne();
                return;
            }*/

            if (Physics.SphereCast(ray, capsuleCollider.radius, out hitInfo, raydistance, groundMask, QueryTriggerInteraction.Ignore))
            {
                if (Vector3.Dot(rb.transform.up, hitInfo.normal) < 0.5f)
                {
                    ExitAirbourne();
                    return;
                }
                Debug.DrawLine(ray.origin, hitInfo.point, Color.cyan, 1);
                //transform.position = transform.position + transform.up * (-Vector3.Dot(transform.up, transform.position) + Vector3.Dot(transform.up, hitInfo.point) + currentCollider.height * 0.5f + BUFFER_DIST);

                rb.position = new Vector3(rb.position.x, hitInfo.point.y + 0.05f + capsuleCollider.height * 0.5f, rb.position.z);
                playerInfo.Normal = hitInfo.normal.normalized;
                rb.velocity = Vector3.ProjectOnPlane(rb.velocity, playerInfo.Normal);
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
            playerInfo.Grounded = false;
            rb.AddForce(-Vector3.Dot(rb.velocity, rb.transform.up) * rb.transform.up * 0.75f, ForceMode.Impulse);
            GetComponentInParent<PlayerStateMachine>().ChangeStates();
        }



    }
}
