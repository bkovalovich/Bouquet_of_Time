using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Bouquet
{
    public class AnimationController : MonoBehaviour
    {

        public Rigidbody rb;
        public Animator animator;
        [SerializeField] PlayerInfoSO playerInfo;

        [SerializeField] PlayerStateMachine playerStateMachine;

        private Vector3 previousVelocity;
        private Vector3 acceleration;

        private bool lastGrounded;

        // Start is called before the first frame update
        void Start()
        {
            previousVelocity = rb.velocity;
        }

        // Update is called once per frame
        void FixedUpdate()
        {

            Vector3 deltaVel = rb.velocity - previousVelocity;
            acceleration = Vector3.Slerp(acceleration, deltaVel / Time.deltaTime, 1 - Mathf.Pow(0.0001f, Time.deltaTime));
            Debug.DrawRay(transform.position + transform.up, acceleration * 5f, Color.green);
            if (playerInfo.Grounded && !lastGrounded)
            {
                animator.SetTrigger("Land");
                animator.SetLayerWeight(animator.GetLayerIndex("Land"), Mathf.Clamp(Mathf.Sqrt(Mathf.Abs(previousVelocity.y)) / 7, 0.1f, 1));
            }
            previousVelocity = rb.velocity;
            lastGrounded = playerInfo.Grounded;
        }

        private void LateUpdate()
        {
            Vector3 localVelocity = transform.worldToLocalMatrix * rb.velocity;
            animator.SetFloat("Speed", rb.velocity.magnitude);
            animator.SetFloat("VelocityX", localVelocity.x);
            animator.SetFloat("VelocityY", Mathf.MoveTowards(animator.GetFloat("VelocityY"), localVelocity.y, 80 * Time.deltaTime));
            animator.SetFloat("VelocityZ", localVelocity.z);
            animator.SetFloat("HorizontalVelocity", Vector3.ProjectOnPlane(rb.velocity, playerInfo.Normal).magnitude);
            float VdotA = Vector3.Dot(Vector3.ProjectOnPlane(rb.velocity, playerInfo.Normal).normalized, Vector3.ProjectOnPlane(acceleration, playerInfo.Normal).normalized);
            animator.SetFloat("VDotA", Mathf.Lerp(animator.GetFloat("VDotA"), VdotA, 1 - Mathf.Pow(0.0001f, Time.deltaTime)));
            float accelAngle = Vector3.SignedAngle(Vector3.ProjectOnPlane(rb.velocity, playerInfo.Normal), Vector3.ProjectOnPlane(acceleration, playerInfo.Normal), playerInfo.Normal);
            animator.SetFloat("AccelerationAngleDifference", Mathf.Lerp(animator.GetFloat("AccelerationAngleDifference"), accelAngle * acceleration.magnitude * 0.05f, 1 - Mathf.Pow(0.0001f, Time.deltaTime)));
            //animator.SetFloat("AccelerationAngleDifference", Mathf.Lerp(animator.GetFloat("AccelerationAngleDifference"), accelAngle * acceleration.magnitude * 0.1f, 1 - Mathf.Pow(0.0001f, Time.deltaTime)));
            animator.SetBool("Grounded", playerInfo.Grounded);

            if (playerStateMachine.CurrentState != null)
            {
                bool islocked = typeof(LockedOnGroundedState).IsAssignableFrom(playerStateMachine.CurrentState.GetType());
                animator.SetLayerWeight(animator.GetLayerIndex("LockedLocomotion"), islocked ? 1 : 0);
            }
            else
            {
                animator.SetLayerWeight(animator.GetLayerIndex("LockedLocomotion"), 0);
            }
            
        }

        public void Setup(PlayerInfoSO info, InputSO input)
        {
            playerInfo = info;
        }



        private void OnAnimatorMove()
        {
            rb.position += animator.deltaPosition;
            transform.parent.rotation = animator.deltaRotation * transform.parent.rotation;
        }
    }
}
