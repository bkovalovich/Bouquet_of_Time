using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] float movementSpeed, mouseSensitivity;
    [SerializeField] Transform cameraTransform;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction move, lookX, lookY;
    private bool isMoving;
    private float xRotation = 0;
    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody>();
        playerInput = new PlayerInput();

    }
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable() {
        move = playerInput.PlayerMove.Move;
        lookX = playerInput.PlayerCameraLook.MouseX;
        lookY = playerInput.PlayerCameraLook.MouseY;
        move.Enable();
        move.started += OnMove;
        move.canceled += OnMoveStop;

        lookX.Enable();
        lookX.performed += OnMouseX;

        lookY.Enable();
        lookY.performed += OnMouseY;
    }
    private void OnDisable() {
        move.Disable();
        move.started -= OnMove;
        move.canceled -= OnMoveStop;

        lookX.Disable();
        lookX.performed -= OnMouseX;
        lookY.Disable();
        lookY.performed -= OnMouseY;
    }
    private void OnMove(InputAction.CallbackContext context) {
        isMoving = true;
    }
    private void OnMoveStop(InputAction.CallbackContext context) {
        isMoving = false;
    }
    private void OnMouseX(InputAction.CallbackContext context) {
        float mouseX = context.ReadValue<float>() * mouseSensitivity * Time.deltaTime;
        gameObject.transform.Rotate(eulers: Vector3.up * mouseX);
    }
    private void OnMouseY(InputAction.CallbackContext context) {
        float mouseY = context.ReadValue<float>() * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(value: xRotation, min: -90f, max: 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, y: 0f, z: 0f);
    }

    private void Movement() {
        if (isMoving) {
            //Vector3 moveVector = new Vector3(, 0, move.ReadValue<Vector2>().y * movementSpeed);
            Vector3 xInput = transform.forward * move.ReadValue<Vector2>().y * movementSpeed;
            Vector3 yInput = transform.right * move.ReadValue<Vector2>().x * movementSpeed;
            rb.velocity = new Vector3(xInput.x + yInput.x, 0, xInput.z + yInput.z);
        } else {
            rb.velocity = Vector3.zero;
        }
    }

    void Update() {
        Movement();
    }
}
