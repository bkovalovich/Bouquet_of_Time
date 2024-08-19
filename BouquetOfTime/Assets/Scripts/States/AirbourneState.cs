using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Bouquet
{
    public class AirbourneState : PlayerState
    {
        [SerializeField] protected float maxSpeed;

        protected float targetSpeed;

        protected float enterSpeed;

        public float acceleration;

        public FloatVariableSO gravityMagnitude;

        [Header("Visuals")]
        [SerializeField] protected Transform modelParent;
        protected Transform pitch;
        protected Transform yaw;
        protected Transform roll;

        public override void EnterState()
        {
            base.EnterState();

            playerInfo.Grounded = false;
            playerInfo.Normal = rb.transform.up;
            enterSpeed = (rb.velocity - playerInfo.Normal * Vector3.Dot(playerInfo.Normal, rb.velocity)).magnitude;

            pitch = modelParent.GetChild(0);
            yaw = pitch.GetChild(0);
            roll = yaw.GetChild(0);
        }

        public override void FrameUpdate()
        {
            CheckGrounded();
            SolveModelRotation();
        }

        public override void PhysicsUpdate()
        {
            rb.AddForce(Vector3.down * gravityMagnitude, ForceMode.Acceleration);


            Move();
        }

        protected virtual void SolveModelRotation()
        {
            pitch.localRotation = Quaternion.Slerp(pitch.localRotation, Quaternion.identity, 1 - Mathf.Pow(0.001f, Time.deltaTime));
            yaw.localRotation = Quaternion.Slerp(yaw.localRotation, Quaternion.identity, 1 - Mathf.Pow(0.001f, Time.deltaTime));
            roll.localRotation = Quaternion.Slerp(roll.localRotation, Quaternion.Euler(0, roll.localRotation.eulerAngles.y, 0), 1 - Mathf.Pow(0.01f, Time.deltaTime));

        }

        public void CheckGrounded()
        {
            Ray ray = new Ray(rb.position - rb.transform.up * playerInfo.currentCollider.height * 0.49f, -rb.transform.up);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 0.2f + rb.velocity.magnitude * Time.deltaTime))
            {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.cyan, 1);
                if (Vector3.Dot(rb.transform.up, hitInfo.normal) >= 0.5f && rb.velocity.y < 0.05f)
                {
                    ExitGrounded();
                }
            }
        }

        protected virtual void Move()
        {
            Vector3 velocity = rb.velocity;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(velocity, playerInfo.Normal);
            targetSpeed = horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed ? enterSpeed : maxSpeed;
            Vector3 y = Vector3.Dot(velocity, playerInfo.Normal) * playerInfo.Normal;
            Vector3 moveDir = new Vector3(input.MoveDirection.x, 0, input.MoveDirection.y);
            Vector3 horizontalCameraDir = cam.transform.forward;
            horizontalCameraDir.y = 0;
            horizontalCameraDir.Normalize();
            Quaternion CameraRotation = Quaternion.FromToRotation(Vector3.forward, horizontalCameraDir);

            moveDir = CameraRotation * moveDir;

            velocity = Vector3.MoveTowards(velocity, Quaternion.FromToRotation(Vector3.up, playerInfo.Normal) * moveDir * targetSpeed + y, acceleration * Time.deltaTime);

            rb.AddForce((velocity - rb.velocity) / Time.deltaTime);

        }

        public void ExitGrounded()
        {
            playerInfo.Grounded = true;
            Debug.Log("ExitGrounded");
            GetComponentInParent<PlayerStateMachine>().ChangeStates();
            //OnGroundedExit?.Invoke();
        }
    }
}
