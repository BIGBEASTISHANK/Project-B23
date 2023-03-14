using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Variables
    /////////////
    private Rigidbody rb;

    [HideInInspector] public Vector3 targetPoint;

    [Header("Values")]
    [SerializeField] private float bulletSpeed;

    // References
    //////////////
    private void Start() { rb = GetComponent<Rigidbody>(); transform.LookAt(targetPoint); } // Getting rigidbody componnt

    // Pusshing bullet forward
    private void FixedUpdate() { rb.velocity = transform.TransformDirection(Vector3.forward * bulletSpeed * Time.fixedDeltaTime); }

    // When colliding anything
    private void OnTriggerEnter(Collider collision) { Destroy(gameObject); }// Destroy bullet
}
