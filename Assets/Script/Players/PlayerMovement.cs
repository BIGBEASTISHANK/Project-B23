using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    /////////////
    public float health;

    private Rigidbody rb;
    private InputSystem inpSys;
    private LayerMask groundLayer;
    private Transform groundcheck;

    [Header("Values")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float groundcheckRadius;

    // References
    //////////////
    private void Awake() { inpSys = new InputSystem(); } // Getting input system

    private void Start()
    {
        // Getting component
        rb = GetComponent<Rigidbody>(); // Getting rigidbody component
        groundLayer = LayerMask.GetMask("Ground"); // Getting ground layer mask
        groundcheck = GameObject.Find("Ground Check").transform; // Getting ground check gameObject
    }

    private void FixedUpdate()
    {
        // Function
        Move(); // Function for movement
        Jump(); // Function for jumping
        GameOver(); // Function for killing player
    }

    // Move function
    private void Move()
    {
        // Reading move value
        Vector2 move = inpSys.Player.Movement.ReadValue<Vector2>().normalized * Time.fixedDeltaTime;

        // Moving player
        if (move != Vector2.zero)
        {
            // Setting up speed
            if (inpSys.Player.Sprint.IsPressed())
                move *= sprintSpeed; // Setting move speed to jump speed
            else move *= walkSpeed; // Setting move speed to walk speed 

            // Moving player
            rb.velocity = transform.TransformDirection(new Vector3(move.x, rb.velocity.y, move.y));
        }
    }

    // Jump function
    private void Jump()
    {
        // Checking if grounded
        bool isGrounded = Physics.CheckSphere(groundcheck.position, groundcheckRadius, groundLayer);

        // Jumping
        if (inpSys.Player.Jump.IsPressed() && isGrounded)
            rb.velocity = Vector3.up * jumpHeight * Time.fixedDeltaTime; // Adding velocity upwards
    }

    // Game over function
    private void GameOver()
    {
        if (health <= 0)
            Debug.Log("GameOver");
    }

    // Enabling input system
    private void OnEnable() { inpSys.Player.Enable(); }
    // Disabling input system
    private void OnDisable() { inpSys.Player.Disable(); }
}
