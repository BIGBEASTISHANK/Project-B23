using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    // Variables
    /////////////
    private InputSystem inpSys;

    [Header("Values")]
    [SerializeField] private float smooth;
    [SerializeField] private float multiplier;

    // References
    //////////////
    private void Awake() { inpSys = new InputSystem(); } // Getting input system

    private void FixedUpdate()
    {
        // get mouse input
        Vector2 mouse = inpSys.Mouse.Move.ReadValue<Vector2>() * multiplier;

        // calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouse.y, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouse.x, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        // rotate 
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.fixedDeltaTime);
    }

    // Enabling input system
    private void OnEnable() { inpSys.Mouse.Enable(); }
    // Disabling input system
    private void OnDisable() { inpSys.Mouse.Disable(); }
}