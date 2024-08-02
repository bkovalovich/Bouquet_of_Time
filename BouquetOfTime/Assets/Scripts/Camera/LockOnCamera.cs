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

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {

        }
    }
}
