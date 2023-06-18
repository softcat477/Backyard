using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance {get; protected set;}

    CharacterController characterController;
    public Transform cameraTransform;

    private PlayerInputActions playerInputActions;
    private InputAction move;
    private InputAction dmouseMove;

    bool isMoving = false;

    [Header("Control Settings")]
    public float playerSpeed = 2.5f;
    public float mouseSensitivity = 2.4f;

    float horizontalAngle = 0.0f;
    float verticalAngle = 0.0f;

    private bool isAiming = false;

    private void Awake() {
        Instance = this;
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable() {
        move = playerInputActions.Player.Move;
        move.performed += OnMovePerformed;
        move.canceled += OnMoveCanceled;
        move.Enable();
        dmouseMove = playerInputActions.Player.DMouseMove;
        dmouseMove.Enable();

        gameObject.GetComponent<PlayerController>().AimEvent += OnAiming;
    }

    private void OnDisable() {
        move.Disable();
        dmouseMove.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        horizontalAngle = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Move with the move action
        if (isMoving) {
            Vector2 moveInput = move.ReadValue<Vector2>();
            Vector3 dMovePerFrame = new Vector3(moveInput.x, 0, moveInput.y);
            dMovePerFrame = dMovePerFrame * playerSpeed * Time.deltaTime;

            dMovePerFrame = transform.TransformDirection(dMovePerFrame);
            characterController.Move(dMovePerFrame);
        }

        // Rotate with mouse action
        if (!isAiming) {
            Vector2 dmouseMoveInput = dmouseMove.ReadValue<Vector2>();
            float dHorizontalAngle = dmouseMoveInput.x * mouseSensitivity;
            float dVerticalAngle = dmouseMoveInput.y * mouseSensitivity;

            // Rotate player horizontally
            horizontalAngle += dHorizontalAngle;
            if (horizontalAngle > 360.0f) horizontalAngle -= 360.0f;
            if (horizontalAngle < 0.0f) horizontalAngle += 360.0f;
            transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, 
                horizontalAngle,
                transform.localEulerAngles.z);

            // Rotate camera vertically
            verticalAngle -= dVerticalAngle;
            verticalAngle = Mathf.Clamp(verticalAngle, -89.0f, 89.0f);
            cameraTransform.localRotation = Quaternion.Euler(verticalAngle, 
                cameraTransform.localEulerAngles.y,
                transform.localEulerAngles.z);
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx) {
        Vector2 tmp = move.ReadValue<Vector2>();
        isMoving = true;
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx) {
        isMoving = false;
    }

    public void OnAiming(bool _isAiming) {
        isAiming = _isAiming;
    }
}
