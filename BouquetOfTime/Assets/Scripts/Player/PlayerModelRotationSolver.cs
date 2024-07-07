using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelRotationSolver : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] AnimationCurve tiltCurve;

    [SerializeField] Transform pitch;
    [SerializeField] Transform yaw;
    [SerializeField] Transform roll;

    [SerializeField] float maxTiltAngle;
    [SerializeField] float accelerationTiltSpeed;
    [SerializeField] float velocityRotationSpeed;

    private Vector3 acceleration;
    private Vector3 previousVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 normalVelocity = rb.velocity.normalized;
        Vector3 velocityCross = Vector3.Cross(normalVelocity, transform.up);

        float tempForward = Vector3.Dot(acceleration, normalVelocity);
        tempForward = tiltCurve.Evaluate(tempForward / (maxTiltAngle * 2)) * maxTiltAngle;
        //tempForward = Mathf.Sqrt(Mathf.Abs(tempForward) * 40) * 0.3f * Mathf.Sign(tempForward);
        float tempSideways = Vector3.Dot(acceleration, velocityCross);
        tempSideways = tiltCurve.Evaluate(tempSideways / (maxTiltAngle * 2)) * maxTiltAngle;
        //tempSideways = Mathf.Sqrt(Mathf.Abs(tempSideways) * 40) * 0.3f * Mathf.Sign(tempSideways);

        var tilt = normalVelocity * tempForward + velocityCross * tempSideways;

        float pitchMagnitude = Vector3.Dot(pitch.forward, tilt);
        float yawMagnitude = Vector3.Dot(yaw.right, tilt);


        /*pitch.localRotation = Quaternion.AngleAxis(pitchMagnitude, pitch.right);
        yaw.localRotation = Quaternion.AngleAxis(-yawMagnitude, yaw.forward);*/
        pitch.localRotation = Quaternion.Slerp(pitch.localRotation, Quaternion.AngleAxis(pitchMagnitude, pitch.right), 1 - Mathf.Pow(accelerationTiltSpeed * 0.01f, Time.deltaTime));
        yaw.localRotation = Quaternion.Slerp(yaw.localRotation, Quaternion.AngleAxis(-yawMagnitude, yaw.forward), 1 - Mathf.Pow(accelerationTiltSpeed * 0.01f, Time.deltaTime)) ;

        if (rb.velocity.sqrMagnitude > 0.1f)
        {
            Vector3 startForward = Vector3.ProjectOnPlane(pitch.forward + yaw.forward, transform.up).normalized;
            Quaternion rollRotation = Quaternion.FromToRotation(startForward, rb.velocity - (Vector3.Dot(Vector3.up, rb.velocity) * Vector3.up));
            roll.localRotation = Quaternion.RotateTowards(roll.localRotation, rollRotation, velocityRotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        acceleration = (rb.velocity - previousVelocity) / Time.deltaTime;
        previousVelocity = rb.velocity;
    }
}
