using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonCharacterController : MonoBehaviour
{
    public InputHandlerSO input;
    public PlayerStateSO playerState;
    public Vector3SO GravityDir;
    public float GravityAmplitude;

    private static float BUFFER_DIST = 0.05f;

    [SerializeField]
    private Rigidbody rb;

    private Camera mainCamera;

    [SerializeField]
    private CapsuleCollider capsuleCollider;
    [SerializeField]
    private CapsuleCollider crouchCollider;

    public CapsuleCollider currentCollider;

    [SerializeField]
    private LayerMask groundMask;

    private Vector3 Normal;
    private Vector3 WallNormal;

    private Vector2 movementInput;

    private bool canJump { get { return playerState.Grounded; } }

    private bool jump;

    private bool sprint;

    private bool previousGrounded;
    private bool previousCrouch;

    [Header("Movement Settings")]
    public float runSpeed;
    public float walkSpeed;
    public float crouchSpeed;
    public float MinSlideSpeed;
    public float SlideSlopeForce;
    private float speed;
    public float JumpHeight;
    public float minJumpHeight;
    private float jumpForce;
    private float minJumpForce;
    private float lastJumpTime;
    private float jumpInputTime;
    public float JumpBufferTime;
    public float SlideForce;
    public float GroundedAcceleration;
    public float AirborneAcceleration;
    public float SlidingAcceleration;
    public float snapDistance;

    [Header("Stair Settings")]
    public float minStairHeight;
    public float maxStairHeight;
    public float minStairDepth;

    [Header("Debug")]
    public bool DoGroundSnap;

    private void Awake()
    {
        jumpForce = Mathf.Sqrt(2 * JumpHeight * GravityAmplitude);
        minJumpForce = Mathf.Sqrt(2 * minJumpHeight * GravityAmplitude);
        jumpInputTime = (jumpForce - minJumpForce) / GravityAmplitude;
        currentCollider = capsuleCollider;
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        input.MoveEvent += MoveInput;
        input.JumpEvent += JumpRequested;
        input.SprintEvent += SprintRequested;
    }

    private void SprintRequested(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            sprint = true;
        }

        if(context.canceled)
        {
            sprint = false;
        }
    }

    private void JumpRequested(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jump = true;
            StartCoroutine(CancelJump(JumpBufferTime));
        }
        if(context.canceled && Time.time - lastJumpTime < jumpInputTime)
        {
            Vector3 verticalVelocity = Vector3.Dot(rb.velocity, -GravityDir.normalized) * -GravityDir.normalized;
            rb.velocity -= verticalVelocity;
            verticalVelocity = -GravityDir.normalized * minJumpForce;
            rb.velocity += verticalVelocity;
            lastJumpTime -= jumpInputTime;
        }
    }

    private IEnumerator CancelJump(float delay)
    {
        yield return new WaitForSeconds(delay);
        jump = false;
    }

    private void MoveInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        input.MoveEvent -= MoveInput;
        input.JumpEvent -= JumpRequested;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateBody();

        UpdateSpeed();

        

        if (Normal.magnitude > 1)
        {
            Debug.Log("Uh oh... the normal on the player is not normalized");
        }
    }

    private void RotateBody()
    {
        transform.up = -GravityDir.normalized;
    }

    private void UpdateSpeed()
    {

        var velocity = Vector3.ProjectOnPlane(rb.velocity, Normal);
        if (!playerState.Grounded && velocity.sqrMagnitude > crouchSpeed * crouchSpeed)
        {
            speed = velocity.magnitude;
            return;
        }

        if (playerState.Crouching)
        {
            speed = crouchSpeed;
            return;
        }

        if(sprint)
        {
            
            speed = runSpeed;
            return;
        }

        speed = walkSpeed;
    }

    

    private void SetGrounded()
    {
        playerState.Grounded = true;
        rb.AddForce(Mathf.Max(0, Vector3.Dot(rb.velocity, GravityDir.normalized)) * -GravityDir.normalized, ForceMode.Impulse);
    }

    

    private void FixedUpdate()
    {
        
        SnapToGround();
        ApplyGravity();
        HandleSlide();
        Move();
        HandleSteps(rb.velocity);
        LookForGround();
        LookForWall();
        TryJump();



        previousGrounded = playerState.Grounded;
        previousCrouch = playerState.Crouching;
    }

    private void LookForWall()
    {
        if (playerState.Grounded) 
        {
            playerState.WallRunning = false;
            return; 
        }

        var horizontalVelocity = Vector3.ProjectOnPlane(rb.velocity, Normal);

        if(horizontalVelocity.sqrMagnitude < 15)
        {
            playerState.WallRunning = false;
            return;
        }


        Ray ray = new Ray(transform.position, rb.velocity.normalized);

        var rayDistance = (rb.velocity.magnitude + BUFFER_DIST) * Time.deltaTime;

        RaycastHit hitInfo;
        if (Physics.SphereCast(ray, currentCollider.radius, out hitInfo, rayDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            if (hitInfo.normal.y < 0.3f)
            {
                playerState.WallRunning = true;
                WallNormal = hitInfo.normal;
            }

            if(Vector3.Dot(horizontalVelocity.normalized, hitInfo.normal) < -0.9f)
            {
                rb.velocity -= horizontalVelocity * 0.95f;
                rb.velocity += transform.up * horizontalVelocity.magnitude * 0.4f;
            }

            
        }

        ray = new Ray(transform.position, Vector3.Cross(transform.up, horizontalVelocity.normalized));

        if (Physics.Raycast(ray, out hitInfo, currentCollider.radius + 0.5f, groundMask, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Dot(rb.velocity.normalized, hitInfo.normal)) < 0.2f)
            {
                if (hitInfo.normal.y < 0.3f)
                {
                    playerState.WallRunning = true;
                    WallNormal = hitInfo.normal;
                    return;
                }
            }
        }

        ray = new Ray(transform.position, Vector3.Cross(-transform.up, horizontalVelocity.normalized));
        if (Physics.Raycast(ray, out hitInfo, currentCollider.radius + 0.5f, groundMask, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Dot(rb.velocity.normalized, hitInfo.normal)) < 0.2f)
            {
                if (hitInfo.normal.y < 0.3f)
                {
                    playerState.WallRunning = true;
                    WallNormal = hitInfo.normal;
                    return;
                }
            }
        }


        WallNormal = Vector3.zero;
        playerState.WallRunning = false;
    }

    private void SnapToGround()
    {
        if(!previousGrounded)
        {
            return;
        }

        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position + GravityDir.normalized * (currentCollider.height * 0.5f - currentCollider.radius - BUFFER_DIST), GravityDir.normalized);

        float raydistance = snapDistance;

        if(!Physics.Raycast(transform.position + GravityDir.normalized * (currentCollider.height * 0.5f), ray.direction, raydistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            return;
        }

        if (Physics.SphereCast(ray, currentCollider.radius, out hitInfo, raydistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.cyan, 1);
            //transform.position = transform.position + transform.up * (-Vector3.Dot(transform.up, transform.position) + Vector3.Dot(transform.up, hitInfo.point) + currentCollider.height * 0.5f + BUFFER_DIST);
            transform.position -= transform.up * (hitInfo.distance - BUFFER_DIST);
            Normal = hitInfo.normal.normalized;
            //rb.velocity = Vector3.ProjectOnPlane(rb.velocity, Normal).normalized * rb.velocity.magnitude;
            //rb.velocity -= Normal * Vector3.Dot(rb.velocity, Normal);
            //rb.velocity -= GravityDir.normalized * Vector3.Dot(rb.velocity, GravityDir.normalized);
            playerState.Grounded = true;
        }
    }

    private void HandleSlide()
    {
        if (!playerState.Crouching)
        {
            playerState.Sliding = false;
            return;
        }

        Vector3 horizontalVelocity = Vector3.ProjectOnPlane(rb.velocity, Normal);

        if (horizontalVelocity.sqrMagnitude <= MinSlideSpeed * MinSlideSpeed)
        {
            playerState.Sliding = false;
            return;
        }

        if(playerState.Sliding && horizontalVelocity.sqrMagnitude <= crouchSpeed * crouchSpeed)
        {
            playerState.Sliding = false;
            return;
        }

        if (!previousCrouch)
        {
            if ((!playerState.Sliding && playerState.Grounded) || (!previousGrounded && playerState.Grounded))
            {
                Debug.Log("Started Sliding");
                rb.AddForce(SlideForce * rb.mass * horizontalVelocity.normalized, ForceMode.Impulse);
            }
            playerState.Sliding = true;
            
        }
    }



    private bool HandleSteps(Vector3 velocity)
    {
        if (!previousGrounded)
        {
            return false;
        }

        Vector3 distanceThisFrame = velocity * Time.deltaTime;
        distanceThisFrame += distanceThisFrame.normalized * BUFFER_DIST;

        //send ray out with distance v * t to see if it hits a wall or bump

        Ray ray = new Ray(transform.position - distanceThisFrame.normalized * BUFFER_DIST - transform.up * (currentCollider.height * 0.5f - currentCollider.radius), distanceThisFrame.normalized);
        RaycastHit hitInfo;
        //spherecast out at MINHEIGHT to see if the player will collide with a wall
        if (!Physics.SphereCast(ray, currentCollider.radius - BUFFER_DIST, out hitInfo, distanceThisFrame.magnitude + BUFFER_DIST, groundMask, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        if (Vector3.Dot(Normal, hitInfo.normal) > 0.7f)
        {
            return false;
        }

        Debug.DrawRay(ray.origin + ray.direction * currentCollider.radius, ray.direction * (distanceThisFrame.magnitude + BUFFER_DIST), Color.blue, 1);

        //if wall/bump is found send another ray to check the height of this bump

        Ray downRay = new Ray(hitInfo.point + transform.up + distanceThisFrame.normalized * BUFFER_DIST, -transform.up);
        Debug.DrawRay(downRay.origin, downRay.direction, Color.red, 1);

        RaycastHit downHit;
        if(!Physics.Raycast(downRay, out downHit, 1 + BUFFER_DIST, groundMask, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        if(downHit.point.y - hitInfo.point.y > maxStairHeight)
        {
            return false;
        }

        distanceThisFrame -= hitInfo.distance * distanceThisFrame.normalized;

        //if the stair is small enough repeat until no more v * t is left

        transform.position += transform.up * (-Vector3.Dot(transform.up, transform.position) + Vector3.Dot(transform.up, hitInfo.point) + currentCollider.height * 0.5f + BUFFER_DIST);
        rb.velocity -= transform.up * Vector3.Dot(transform.up, rb.velocity);

        int iterations = 0;

        while (distanceThisFrame.sqrMagnitude > 0 && iterations < 10)
        {
            Debug.Log($"frame dist: {distanceThisFrame} hitInfo.distance: {hitInfo.distance}");

            ray.origin = downHit.point - distanceThisFrame.normalized * BUFFER_DIST + transform.up * (currentCollider.radius);
            ray.direction = distanceThisFrame.normalized;

            Debug.DrawRay(ray.origin + ray.direction * currentCollider.radius, ray.direction * (distanceThisFrame.magnitude + BUFFER_DIST), Color.blue, 1);
            if (!Physics.SphereCast(ray, currentCollider.radius - BUFFER_DIST, out hitInfo, distanceThisFrame.magnitude + BUFFER_DIST * 2, groundMask, QueryTriggerInteraction.Ignore))
            {
                return true;
            }

            if (Vector3.Dot(Normal, hitInfo.normal) > 0.7f)
            {
                return true;
            }

            /*if (hitInfo.distance < minStairDepth)
            {
                return true;
            }*/

            downRay = new Ray(hitInfo.point + transform.up + distanceThisFrame.normalized * BUFFER_DIST, -transform.up);
            Debug.DrawRay(downRay.origin, downRay.direction, Color.green, 1);

            if (!Physics.Raycast(downRay, out downHit, 1 + BUFFER_DIST, groundMask, QueryTriggerInteraction.Ignore))
            {
                return true;
            }

            if (downHit.point.y - hitInfo.point.y > maxStairHeight)
            {
                return true;
            }

            distanceThisFrame -= hitInfo.distance * distanceThisFrame.normalized;

            //if the stair is small enough repeat until no more v * t is left

            iterations++;

            transform.position += transform.up * (-Vector3.Dot(transform.up, transform.position) + Vector3.Dot(transform.up, hitInfo.point) + currentCollider.height * 0.5f + BUFFER_DIST * 2);
        }

        return true;
    }

    private void LookForGround()
    {
        Ray ray = new Ray(transform.position + GravityDir.normalized * (currentCollider.height * 0.5f - currentCollider.radius), GravityDir.normalized);

        float raydistance = playerState.Grounded ? BUFFER_DIST * 3 : BUFFER_DIST * 2;

        RaycastHit hitInfo;

        if(Physics.SphereCast(ray, currentCollider.radius - BUFFER_DIST, out hitInfo, raydistance + BUFFER_DIST, groundMask, QueryTriggerInteraction.Ignore))
        {
            float dot = Vector3.Dot(hitInfo.normal, -GravityDir.normalized);
            if (dot >= 0.5f)
            {
                Normal = hitInfo.normal.normalized;
                playerState.Grounded = true;
                if (!playerState.Grounded)
                {
                    SetGrounded();
                }
                return;
            }
        }

        Normal = -GravityDir.normalized;
        playerState.Grounded = false;
    }

    private void ApplyGravity()
    {
        if (playerState.Grounded) { return; }

        if(playerState.WallRunning)
        {
            var amplitude = Vector3.Dot(rb.velocity, GravityDir.normalized) < 0 ? GravityAmplitude : GravityAmplitude * (0.75f + Vector3.Dot(GravityDir.normalized, WallNormal));
            rb.AddForce(GravityDir.normalized * amplitude, ForceMode.Acceleration);
            return;
        }

        rb.AddForce(GravityDir.normalized * GravityAmplitude, ForceMode.Acceleration);
    }

    private void TryJump()
    {
        if(jump && canJump)
        {
            var force = playerState.Crouching ? jumpForce * 0.75f : jumpForce;

            rb.AddForce(force * rb.mass * Vector3.Lerp(-GravityDir.normalized, Normal, 0.25f), ForceMode.Impulse);
            jump = false;
            playerState.Grounded = false;
            previousGrounded = false;
            lastJumpTime = Time.time;
        }
        else if(jump && playerState.WallRunning)
        {
            var dot = Vector3.Dot(rb.velocity, GravityDir.normalized);
            if (dot > 0)
            {
                rb.velocity -= GravityDir.normalized * dot;
            }
            var force = (transform.up + WallNormal) * 0.5f;
            rb.AddForce(force * rb.mass * jumpForce * 1.35f, ForceMode.Impulse);
            jump = false;
            playerState.WallRunning = false;
            playerState.Grounded = false;
            previousGrounded = false;
            lastJumpTime = Time.time;
        }
    }

    private void Move()
    {
        /*if(playerState.CurrentState == PlayerMoveState.WallRunning)
        {
            WallMove();
        }
        else if (playerState.Sliding)
        {
            SlideMove();
        }
        else if(playerState.CurrentState == PlayerMoveState.Airborne)
        {
            AirborneMove();
        }*/
        else if(playerState.Grounded)
        {
            GroundedMove();
        }
    }

    private void GroundedMove()
    {
        if(Time.deltaTime == 0) { return; }

        var velocity = rb.velocity;

        Vector3 verticalComponent = Normal * Vector3.Dot(Normal, velocity);
        //velocity -= verticalComponent;


        Vector3 CameraForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, -GravityDir.normalized);
        CameraForward = CameraForward - (Vector3.Dot(CameraForward, Normal) / Vector3.Dot(-GravityDir.normalized, Normal)) * -GravityDir.value;
        //CameraForward = Quaternion.FromToRotation(-GravityDir.value, Normal) * CameraForward;
        Debug.DrawRay(transform.position, CameraForward * 2, Color.red);

        Vector3 rotatedInput = Quaternion.LookRotation(CameraForward, Normal) * new Vector3(movementInput.x, 0, movementInput.y);


        velocity = Vector3.MoveTowards(velocity, rotatedInput * speed, GroundedAcceleration * Time.deltaTime);

        //velocity += verticalComponent;
        //rb.velocity = velocity;

        rb.AddForce((velocity - rb.velocity) / Time.deltaTime);
    }

    private void AirborneMove()
    {
        if (Time.deltaTime == 0) { return; }

        var velocity = rb.velocity;

        Vector3 verticalComponent = Normal * Vector3.Dot(Normal, velocity);
        velocity -= verticalComponent;

        Vector3 CameraForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, -GravityDir.value).normalized;
        Debug.DrawRay(transform.position, CameraForward * 2, Color.red);

        Vector3 rotatedInput = Quaternion.LookRotation(CameraForward, Normal) * new Vector3(movementInput.x, 0, movementInput.y);


        velocity = Vector3.MoveTowards(velocity, rotatedInput * speed, AirborneAcceleration * Time.deltaTime);

        velocity += verticalComponent;
        //rb.velocity = velocity;

        rb.AddForce((velocity - rb.velocity) / Time.deltaTime);
    }

    private void SlideMove()
    {
        if (Time.deltaTime == 0) { return; }

        var velocity = rb.velocity;

        Vector3 verticalComponent = Normal * Vector3.Dot(Normal, velocity);
        velocity -= verticalComponent;


        Vector3 CameraForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, -GravityDir.normalized);
        CameraForward = CameraForward - (Vector3.Dot(CameraForward, Normal) / Vector3.Dot(-GravityDir.normalized, Normal)) * -GravityDir.value;
        //CameraForward = Quaternion.FromToRotation(-GravityDir.value, Normal) * CameraForward;
        Debug.DrawRay(transform.position, CameraForward * 2, Color.red);

        Vector3 rotatedInput = Quaternion.LookRotation(CameraForward, Normal) * new Vector3(movementInput.x, 0, movementInput.y);


        velocity = Vector3.MoveTowards(velocity, rotatedInput * speed, SlidingAcceleration * Time.deltaTime);

        if (playerState.Grounded)
        {
            velocity *= 0.97f;

            Vector3 slideDirection = Normal - GravityDir.normalized;

            rb.AddForce(slideDirection * rb.mass * SlideSlopeForce, ForceMode.Acceleration);
        }

        velocity += verticalComponent;
        //rb.velocity = velocity;

        rb.AddForce((velocity - rb.velocity) / Time.deltaTime);
    }

    private void WallMove()
    {
        if (Time.deltaTime == 0) { return; }

        var velocity = rb.velocity;

        Vector3 verticalComponent = Normal * Vector3.Dot(Normal, velocity);
        velocity -= verticalComponent;

        Vector3 CameraForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, -GravityDir.value).normalized;
        Debug.DrawRay(transform.position, CameraForward * 2, Color.red);

        Vector3 rotatedInput = Quaternion.LookRotation(CameraForward, Normal) * new Vector3(movementInput.x, 0, movementInput.y);


        velocity = Vector3.MoveTowards(velocity, rotatedInput * speed, AirborneAcceleration * 0.5f * Time.deltaTime);

        velocity += verticalComponent;
        //rb.velocity = velocity;

        rb.AddForce((velocity - rb.velocity) / Time.deltaTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        Vector3 CameraForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, -GravityDir.normalized).normalized;
        CameraForward = CameraForward - (Vector3.Dot(CameraForward, Normal) / Vector3.Dot(-GravityDir.normalized, Normal)) * -GravityDir.value;


        Vector3 rotatedInput = Quaternion.FromToRotation(Vector3.forward, CameraForward) * new Vector3(movementInput.x, 0, movementInput.y);
        Debug.DrawRay(transform.position, rotatedInput * 2, Color.red);

        if (HandleSteps(rotatedInput.normalized * (minStairDepth + BUFFER_DIST)))
        {
            transform.position += rotatedInput * minStairDepth;
        }
    }
}
