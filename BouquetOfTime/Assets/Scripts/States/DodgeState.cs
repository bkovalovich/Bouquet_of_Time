using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Bouquet
{
    public class DodgeState : PlayerState
    {
        [SerializeField] Animator animator;
        [SerializeField] Transform model;

        [SerializeField] float dodgeTime;
        [SerializeField] float directionInputTime;

        protected Vector3 projectedInput;



        public void OnDodgeFinished()
        {
            GetComponentInParent<PlayerStateMachine>().TransitionOut();
        }

        public override void EnterState()
        {
            base.EnterState();
            animator.CrossFade("Dodge", 0.25f);

            projectedInput = input.MoveDirection.sqrMagnitude > 0 ? new Vector3(input.MoveDirection.x, 0, input.MoveDirection.y).normalized : Vector3.forward;
            Vector3 CameraForward = cam.transform.forward;
            CameraForward = Vector3.ProjectOnPlane(CameraForward, rb.transform.up).normalized;
            projectedInput = Quaternion.FromToRotation(Vector3.forward, CameraForward) * projectedInput;
            projectedInput.Normalize();

        }

        public override void FrameUpdate()
        {
            if(Time.time - timeEntered < directionInputTime && input.MoveDirection.sqrMagnitude > 0)
            {
                projectedInput = new Vector3(input.MoveDirection.x, 0, input.MoveDirection.y).normalized;
                Vector3 CameraForward = cam.transform.forward;
                CameraForward = Vector3.ProjectOnPlane(CameraForward, rb.transform.up).normalized;
                projectedInput = Quaternion.FromToRotation(Vector3.forward, CameraForward) * projectedInput;
                projectedInput.Normalize();
                float y = rb.velocity.y;
                rb.velocity = projectedInput * ((rb.velocity - rb.transform.up * y).magnitude);
                rb.velocity = new Vector3(rb.velocity.x, y, rb.velocity.z);
            }

            Transform roll = model.GetChild(0).GetChild(0).GetChild(0);
            Debug.DrawRay(transform.position, projectedInput * 2, Color.yellow);
            roll.localRotation = Quaternion.Slerp(roll.localRotation, Quaternion.LookRotation(projectedInput, rb.transform.up), 1 - Mathf.Pow(0.01f, Time.deltaTime));
        }

        public override void PhysicsUpdate()
        {
            rb.AddForce(Vector3.down * 12, ForceMode.Acceleration);
            float y = rb.velocity.y;
            rb.velocity *= 0.95f;
            rb.velocity = new Vector3(rb.velocity.x, y, rb.velocity.z);
        }
    }
}
