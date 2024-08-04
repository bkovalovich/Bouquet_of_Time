using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Bouquet
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class LockOnCamera : MonoBehaviour
    {
        [SerializeField] CinemachineFreeLook cinemachineFreeLook;

        public Transform target;

        public Vector2 targetOffset;

        public LayerMask targetsMask;

        public bool debug;

        private void Awake()
        {
            if (!cinemachineFreeLook)
            {
                cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
            }
            if(!target)
            {
                target = transform.GetChild(0);
            }
        }

        private void OnEnable()
        {
            
        }

        public void LookForTarget()
        {
            Collider[] targets = Physics.OverlapSphere(transform.parent.position, 20, targetsMask);
            if(targets.Length == 0) { LockOff();  return; }

            Collider closest = targets[0];
            foreach (Collider collider in targets)
            {
                float distance = Vector3.Distance(transform.parent.position, collider.transform.position);
                if(distance < Vector3.Distance(transform.parent.position, closest.transform.position))
                {
                    closest = collider;
                }
            }
            target.parent = closest.transform;
            target.localPosition = Vector3.zero;
            DoLockOn();
        }

        private void DoLockOn()
        {
            if (target.parent == transform) 
            {
                LockOff();
                return; 
            }
            cinemachineFreeLook.enabled = true;
            cinemachineFreeLook.Priority = 2;

        }

        private void LockOff()
        {
            target.parent = transform;
            target.localPosition = Vector3.zero;
            cinemachineFreeLook.enabled = false;
            cinemachineFreeLook.Priority = -10;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            if(debug)
            {
                debug = false;
                if(target.parent != transform)
                {
                    LockOff();
                    return;
                }

                LookForTarget();
            }
        }
    }
}
