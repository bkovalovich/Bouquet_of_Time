using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bouquet
{
    public class SprintAttackState : CombatState
    {


        [SerializeField] Transform model;

        protected Vector3 projectedInput;
        [SerializeField] float directionInputTime;



        public override void EnterState()
        {
            base.EnterState();
            animator.SetTrigger("SprintAttack");
            animator.ResetTrigger("DoNextAttack");

            projectedInput = new Vector3(rb.velocity.x, 0, rb.velocity.y).normalized;
            
        }

        public void OnAttackFinished()
        {
            ExitAttackFinished();
        }

        public override void FrameUpdate()
        {
            

            Transform roll = model.GetChild(0).GetChild(0).GetChild(0);
            Debug.DrawRay(transform.position, projectedInput * 2, Color.black);
            //roll.localRotation = Quaternion.Slerp(roll.localRotation, Quaternion.LookRotation(projectedInput, rb.transform.up), 1 - Mathf.Pow(0.01f, Time.deltaTime));
        }

        public override void PhysicsUpdate()
        {
            if (Time.time - timeEntered < directionInputTime && input.MoveDirection.sqrMagnitude > 0)
            {
                Vector3 CameraForward = cam.transform.forward;
                CameraForward = Vector3.ProjectOnPlane(CameraForward, rb.transform.up).normalized;
                Vector3 localizedInput = Quaternion.FromToRotation(Vector3.forward, CameraForward) * new Vector3(input.MoveDirection.x, 0, input.MoveDirection.y).normalized;
                Debug.DrawRay(transform.position, localizedInput * 2, Color.black);
                projectedInput = Vector3.Lerp(projectedInput, localizedInput, 1 - Mathf.Pow(0.001f, Time.deltaTime));
                //projectedInput = Quaternion.FromToRotation(Vector3.forward, CameraForward) * projectedInput;
                //projectedInput.Normalize();
                float y = rb.velocity.y;
                rb.velocity = Vector3.Project(projectedInput, rb.velocity);
                rb.velocity = new Vector3(rb.velocity.x, y, rb.velocity.z);
            }

            rb.velocity *= 0.95f;

            rb.AddForce(Vector3.down * gravityMagnitude, ForceMode.Acceleration);
        }

        protected virtual void ExitAttackFinished()
        {
            GetComponentInParent<PlayerStateMachine>().TransitionOut();
        }
    }
}
