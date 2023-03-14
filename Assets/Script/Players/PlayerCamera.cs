using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Variables
    /////////////
    private Transform player;
    private InputSystem inpSys;

    [Header("Values")]
    [SerializeField] private float xRotation;
    [SerializeField] private float mouseSensitivity;

    // References
    //////////////
    private void Awake() { inpSys = new InputSystem(); } // Getting input system

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locking cursor

        // Getting components
        player = GameObject.FindWithTag("Player").transform; // Getting player body
    }

    private void FixedUpdate()
    {
        // Getting mouse values
        Vector2 mouse = inpSys.Mouse.Move.ReadValue<Vector2>() * mouseSensitivity * Time.fixedDeltaTime;

        // Getting where to move mouse.
        xRotation -= mouse.y;
        // Stop mouse rotation to get beyond one point.
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        // Rotating player for rotating side by side.
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotating camera up and down.
        player.Rotate(Vector3.up * mouse.x);
    }

    // Enabling input system
    private void OnEnable() { inpSys.Mouse.Enable(); }
    // Disabling input system
    private void OnDisable() { inpSys.Mouse.Disable(); }
}
