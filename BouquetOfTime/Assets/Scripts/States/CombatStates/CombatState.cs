using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Bouquet
{
    public class CombatState : PlayerState
    {
        [SerializeField] protected FloatVariableSO gravityMagnitude;

        public Animator animator;
        public GameObject hitBox;
        public Transform modelParentTransform;
        public LayerMask importantObjectMask;
        public CombatState nextState;

        private Vector3 AttackDir;

        public override void EnterState()
        {
            base.EnterState();
            animator.SetTrigger("DoNextAttack");
            Vector3 position = FindImportantObject();
            if (position != Vector3.zero)
            {
                Debug.Log("Attack: enemy");
                AttackDir = position - transform.position;
                AttackDir.y = 0;
            }
            else if(input.MoveDirection.sqrMagnitude > 0.1f)
            {
                Debug.Log("Attack: input");
                AttackDir = new Vector3(input.MoveDirection.x, 0, input.MoveDirection.y);
                Vector3 forward = playerInfo.camera.transform.forward;
                forward.y = 0;
                forward.Normalize();
                AttackDir = Quaternion.LookRotation(forward, Vector3.up) * AttackDir;
            }
            else
            {
                Debug.Log("Attack: forward");
                AttackDir = modelParentTransform.forward;
            }

        }

        private Vector3 FindImportantObject()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, importantObjectMask, QueryTriggerInteraction.Ignore);

            Vector3 position = Vector3.zero;
            float minWeight = 0;

            foreach(Collider collider in colliders)
            {
                Vector3 direction = collider.transform.position - transform.position;
                float distance = Vector3.Distance(transform.position, transform.position);

                if(distance < Vector3.Distance(position, transform.position))
                {
                    position = collider.transform.position;
                }
            }

            return position;
        }

        public override void FrameUpdate()
        {
            modelParentTransform.localRotation = Quaternion.Slerp(modelParentTransform.localRotation, Quaternion.LookRotation(AttackDir, rb.transform.up), 1 - Mathf.Pow(0.001f, Time.deltaTime));
        }

        public override void PhysicsUpdate()
        {
            float y = rb.velocity.y;
            rb.velocity *= 0.75f;
            rb.velocity = new Vector3(rb.velocity.x, y, rb.velocity.z);

            rb.AddForce(Vector3.down * gravityMagnitude, ForceMode.Acceleration);
        }

        public void OnAttackCompleted()
        {

        }

        public void OnWindUpFinished()
        {

        }

        public void OnStartHitbox(float time)
        {
            GameObject obj = Instantiate(hitBox);
            obj.transform.position = transform.position + animator.gameObject.transform.forward * 1.5f;
            obj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, animator.gameObject.transform.forward);
            HitboxObject hitObject = obj.GetComponent<HitboxObject>();
            if(hitObject != null) { return; }
            hitObject.hitBoxTime = time;

        }
    }
}
