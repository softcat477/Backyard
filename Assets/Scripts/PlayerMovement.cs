using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance {get; protected set;}

    CharacterController characterController;

    private PlayerInputActions playerInputActions;
    private InputAction move;

    bool isMoving = false;

    [Header("Control Settings")]
    public float playerSpeed = 2.5f;

    private void Awake() {
        Instance = this;
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable() {
        move = playerInputActions.Player.Move;
        move.performed += OnMovePerformed;
        move.canceled += OnMoveCanceled;
        move.Enable();
    }

    private void OnDisable() {
        move.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx) {
        Vector2 tmp = move.ReadValue<Vector2>();
        isMoving = true;
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx) {
        isMoving = false;
    }
}
