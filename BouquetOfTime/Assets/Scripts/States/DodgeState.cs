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

        protected Vector2 inputDir;

        [SerializeField] EventSO dodgeFinishedEvent;

        protected virtual void OnEnable()
        {
            animator.CrossFade("Dodge", 0.25f);
            dodgeFinishedEvent.Subscribe(OnDodgeFinished);
        }

        private void OnDodgeFinished()
        {
            GetComponentInParent<PlayerStateMachine>().TransitionOut();
        }

        private void OnDisable()
        {
            dodgeFinishedEvent.Unsubscribe(OnDodgeFinished);
        }

        public override void EnterState()
        {
            base.EnterState();
            Vector3 projectedInput = Vector3.forward;
            projectedInput = Camera.main.transform.rotation * projectedInput;
            inputDir = new Vector2(projectedInput.x, projectedInput.y).normalized;
        }

        public override void FrameUpdate()
        {
            if(Time.time - timeEntered > directionInputTime && input.MoveDirection.sqrMagnitude > 0)
            {
                Vector3 projectedInput = new Vector3(input.MoveDirection.x, 0, input.MoveDirection.y);
                projectedInput = Camera.main.transform.rotation * projectedInput;
                inputDir = new Vector2(projectedInput.x, projectedInput.y).normalized;
            }

            Transform roll = model.GetChild(0).GetChild(0).GetChild(0);

            roll.localRotation = Quaternion.FromToRotation(Vector3.forward, new Vector3(inputDir.x, 0, inputDir.y));
        }

        public override void PhysicsUpdate()
        {
            rb.AddForce(Vector3.down * 12, ForceMode.Acceleration);

            rb.velocity *= 0.95f;
        }
    }
}
