using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelRotationSolver : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] Transform pitch;
    [SerializeField] Transform yaw;
    [SerializeField] Transform roll;

    [SerializeField] float maxTiltAngle;
    [SerializeField] float accelerationTiltSpeed;
    [SerializeField] float velocityRotationSpeed;

    private float forwardTilt;
    private float sidewaysTilt;

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
        tempForward = Mathf.Sqrt(Mathf.Abs(tempForward) * 20) * 1 * Mathf.Sign(tempForward);
        float tempSideways = Vector3.Dot(acceleration, velocityCross);
        tempSideways = Mathf.Sqrt(Mathf.Abs(tempSideways) * 20) * 1 * Mathf.Sign(tempSideways);

        forwardTilt = Mathf.MoveTowards(forwardTilt, tempForward, accelerationTiltSpeed * Time.deltaTime);
        sidewaysTilt = Mathf.MoveTowards(sidewaysTilt, tempSideways, accelerationTiltSpeed * Time.deltaTime);


        var tilt = normalVelocity * forwardTilt + velocityCross * sidewaysTilt;
        /*float tiltMagnitude = tilt.magnitude;
        tilt = tilt.normalized * Mathf.Lerp(tiltMagnitude, maxTiltAngle, tiltMagnitude / (maxTiltAngle));*/

        float pitchMagnitude = Vector3.Dot(pitch.forward, tilt);
        float yawMagnitude = Vector3.Dot(yaw.right, tilt);
        
        pitch.localRotation = Quaternion.AngleAxis(pitchMagnitude, pitch.right);
        yaw.localRotation = Quaternion.AngleAxis(-yawMagnitude, yaw.forward);

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
