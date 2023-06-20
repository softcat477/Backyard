using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FireStatus
{
    None, Aim, Fire
}

/// <summary>
/// This class controls the player's input and manages the player's interaction with the game world,
/// including aiming and firing projectiles using a drag mechanism.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private InputAction leftClick;
    private InputAction mouseMove;

    // The click position (screen space) when the user enters the Aim state and moves to the Fire state in the next update
    private Vector2 leftClickOrigin;

    // Instantiate a new <ballPrefab> GameObject at position + prefabOffset (world space) and let <launchedBullet> keep it
    public GameObject ballPrefab;
    private GameObject launchedBullet;
    public Vector3 prefabOffset = new Vector3(-3.25f, 3.49f, 4.26f);

    [SerializeField] private TrajectorySimulator trajectorySimulator;
    private FireStatus fireStatus;
    // Apply force to <launchedBullet> and use the force to simulate its trajectory
    private Vector3 force;

    // Drag GUI to visualize drag on screen
    //public GameObject dragVisualizer;

    public delegate void AimEventDelegate(bool isAiming);
    public event AimEventDelegate AimEvent;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        fireStatus = FireStatus.None;
        launchedBullet = null;
        //dragVisualizer.SetActive(false);

        if (!ballPrefab.CompareTag("Projectile")) {
            Debug.LogError("TrajectorySimulator.cs requires ballPrefab to be tagged as Projectile. Get tag: " + ballPrefab.tag);
        }
    }

    private void OnEnable() {
        leftClick = playerInputActions.Player.LeftClick;
        leftClick.Enable();
        leftClick.performed += OnLeftClick;
        leftClick.canceled += OnLeftRelease;

        mouseMove = playerInputActions.Player.MouseMove;
        mouseMove.Enable();
    }

    private void OnDisable() {
        leftClick.Disable();
        mouseMove.Disable();
    }

    private void Update() {
        if (fireStatus == FireStatus.Aim) {
            Vector2 releasePosition = mouseMove.ReadValue<Vector2>();
            Vector2 diff = releasePosition - leftClickOrigin;

            // Visualize the mouse movement
            //dragVisualizer.GetComponent<DragVisualizer>().SetTrace(releasePosition);

            // Use x's offset as the rotated degree of the forward direction
            float degree = Mathf.Clamp(diff.x, -200.0f, 200.0f) * 90 / 200;

            // Use y's offset as the magnitude
            float magnitude = Mathf.Clamp(diff.y, -Mathf.Sqrt(200*200*2), Mathf.Sqrt(200*200*2));
            magnitude = magnitude / Mathf.Sqrt(200*200*2) * 8 + 8;

            // Instantiate and hold a new bullet
            if (launchedBullet == null) {
                launchedBullet = Instantiate(ballPrefab, transform.TransformPoint(prefabOffset), transform.rotation);
                launchedBullet.transform.SetParent(transform);
                launchedBullet.GetComponent<Rigidbody>().isKinematic = true; // to stop physics simulation or else the projectile will fall
            }

            // Calculate the applied force. The bullet is always fired at an angle of 45 degree with the horizontal
            force = Quaternion.AngleAxis(-45, launchedBullet.transform.right) * launchedBullet.transform.forward * magnitude;
            // Rotate w.r.t the up vector of the bullet
            force = Quaternion.AngleAxis(degree, launchedBullet.transform.up) * force;

            // Simulate the trajectory using the applied force
            OnSimulate(force, launchedBullet.GetComponent<Rigidbody>().mass, launchedBullet.transform);
        }
        else if (fireStatus == FireStatus.Fire) {
            launchedBullet.transform.SetParent(transform.parent);
            Rigidbody rb = launchedBullet.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(force, ForceMode.Impulse);
            fireStatus = FireStatus.None;
            launchedBullet = null;
        }
    }

    /// <summary>
    /// The callback function when the user clicks the left mouse button. 
    /// Save the click position to calculate the force applied to the bullet.
    /// Activate the Drag GUI to visualize the mouse movement.
    /// Move the GUI to be centered at the click position.
    /// </summary>
    private void OnLeftClick(InputAction.CallbackContext ctx) {
        leftClickOrigin = mouseMove.ReadValue<Vector2>();
        fireStatus = FireStatus.Aim;
        AimEvent?.Invoke(true);

        //dragVisualizer.GetComponent<DragVisualizer>().SetAnchor(leftClickOrigin);
        //dragVisualizer.SetActive(true);
    }

    /// <summary>
    /// The callback function when the user releases the left mouse button.
    /// </summary>
    private void OnLeftRelease(InputAction.CallbackContext ctx) {
        fireStatus = FireStatus.Fire;
        AimEvent?.Invoke(false);
        //dragVisualizer.SetActive(false);
    }

    private void OnSimulate(Vector3 force, float mass, Transform prefabTransform) {
        trajectorySimulator.StartSimulate(force, mass, prefabOffset, prefabTransform);
    }
}
