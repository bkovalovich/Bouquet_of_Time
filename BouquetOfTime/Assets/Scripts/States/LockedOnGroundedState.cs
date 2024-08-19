using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bouquet
{
    public class LockedOnGroundedState : GroundedState
    {

        [Header("Camera")]
        [SerializeField] Transform target;

        protected override void SolveModelRotation()
        {
            Vector3 startForward = Vector3.ProjectOnPlane(pitch.forward + yaw.forward, transform.up).normalized;
            Vector3 lookDir = target.position - transform.position;
            lookDir = Vector3.ProjectOnPlane(lookDir, transform.up).normalized;
            Quaternion rollRotation = Quaternion.FromToRotation(startForward, lookDir).normalized;
            roll.localRotation = Quaternion.Slerp(roll.localRotation, rollRotation, 1 - Mathf.Pow(0.001f, Time.deltaTime));
            Debug.DrawRay(transform.position, rollRotation * Vector3.forward * 3, Color.yellow);
        }
    }
}
