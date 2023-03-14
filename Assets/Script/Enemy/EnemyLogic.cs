using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    // Variables
    /////////////
    public float health;

    private bool isChasing;
    private bool isAttacking;
    private bool walkPointSet;
    private bool alreadyAttacked;
    private bool playerInSightRange;
    private bool playerInAttackRange;

    private Animator anim;
    private Vector3 walkPoint;
    private NavMeshAgent agent;
    private Transform playerCam;
    private GameObject targetPoint;

    [Header("Values")]
    [SerializeField] private float damage;
    [SerializeField] private float fireRate;
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float chasingSpeed;
    [SerializeField] private float patrolingSpeed;
    [SerializeField] private float walkPointRange;
    [SerializeField] private float waitBeforePatrol;
    [SerializeField] private string animDeathString;
    [SerializeField] private string animWalkingString;
    [SerializeField] private string animRunningString;
    [SerializeField] private string animGunplayString;

    [Header("Components")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private TMP_Text damageTxt;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AnimationClip deathAnimation;
    [SerializeField] private AudioSource gunShotSound;

    // References
    //////////////
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Getting navmesh agent
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").transform; // Getting player body
        anim = GetComponent<Animator>(); // Getting animator component
        targetPoint = GameObject.Find("Enemy Shoot Target");
    }

    private void FixedUpdate()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer); // Checking if player is in sight range
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer); // Checking if player is in attack range

        if (!playerInSightRange && !playerInAttackRange) Patroling(); // Start petroling
        if (playerInSightRange && !playerInAttackRange) ChasePlayer(); // Chasing player
        if (playerInSightRange && playerInAttackRange) AttackPlayer(); // Attacking player

        // Chase & Attack animation
        if (isChasing) anim.SetBool(animRunningString, true); else anim.SetBool(animRunningString, false); // Chase animation
        if (isAttacking) anim.SetBool(animGunplayString, true); else anim.SetBool(animGunplayString, false); // Attack animation

        // Damage
        HealthShow(); // Showing health
        Death(); // Death function
    }

    // Patroling function
    private void Patroling()
    {
        // Initial setup
        isChasing = false;
        isAttacking = false;
        agent.speed = patrolingSpeed; // Setting AI speed to patroling

        // Logics
        if (!walkPointSet || !agent.CalculatePath(walkPoint, agent.path)) SearchWalkPoint(); // If no walkpoint is set find walk point
        else if (walkPointSet && agent.CalculatePath(walkPoint, agent.path))
        { agent.SetDestination(walkPoint); anim.SetBool(animWalkingString, true); } // Moving enemy to patrol position and animating

        // Getting distence left to walk
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude <= 0.211f)
        { Invoke(nameof(WaitAtPoint), waitBeforePatrol); anim.SetBool(animWalkingString, false); } // Waiting to patrol again and stoping animation

        // Serching walk point
        void SearchWalkPoint()
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange); // Random z range
            float randomX = Random.Range(-walkPointRange, walkPointRange); // Random x range

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ); // Getting random location

            // Setting walkpoint to true
            if (Physics.Raycast(walkPoint, -transform.up, Mathf.Infinity, groundLayer))
                walkPointSet = true;
        }
    }

    // Wait at point function
    private void WaitAtPoint() { walkPointSet = false; CancelInvoke(nameof(WaitAtPoint)); } // Setting walkpoint to false

    // Chase function
    private void ChasePlayer()
    {
        // Initial setup
        isChasing = true;
        isAttacking = false;
        anim.SetBool(animWalkingString, false); // Stoping walking animation
        agent.speed = chasingSpeed; //Setting AI speed to chasing

        // Logics 
        agent.SetDestination(playerCam.position); // Going to player
    }

    // Attack function
    private void AttackPlayer()
    {
        // Initial setup
        isChasing = false;
        isAttacking = true;
        anim.SetBool(animWalkingString, false); // Stoping walking animation

        // Stopping enemy from moving
        agent.SetDestination(transform.position);
        transform.LookAt(playerCam); // Looking at player

        // If not attacked then attack
        if (!alreadyAttacked)
        {
            // Shooting function
            Shoot();

            alreadyAttacked = true; // If attacked
            // Setting fireRate
            Invoke(nameof(ResetAttack), fireRate);
        }
    }

    //Shoot function
    private void Shoot()
    {
        // Spawning bullet
        GameObject shootBullet = Instantiate(bullet, shootPoint.position, shootPoint.localRotation);
        shootBullet.SetActive(true); // Enabling bullet
        gunShotSound.Play(); // Playing shooting sound

        // Getting bullet script
        Bullet enemyBullet = shootBullet.GetComponent<Bullet>();
        enemyBullet.targetPoint = targetPoint.transform.position;

        // Getting player script
        PlayerMovement playerScript = targetPoint.GetComponentInParent<PlayerMovement>();
        playerScript.health -= damage;
    }

    // Resetting attack
    private void ResetAttack() { alreadyAttacked = false; CancelInvoke(nameof(ResetAttack)); }

    // Healthshow function
    private void HealthShow() { damageTxt.text = health.ToString(); } // Displaying health

    // Death function
    private void Death()
    {
        // Checking health
        if (health <= 0)
        {
            damageTxt.text = "0";
            //Stoping all other animations
            isAttacking = false;
            isChasing = false;
            anim.SetBool(animGunplayString, false);
            anim.SetBool(animRunningString, false);
            anim.SetBool(animWalkingString, false);
            anim.SetBool(animDeathString, true);
            Invoke(nameof(Kill), deathAnimation.averageDuration + 0.1f);
        }
    }

    // Kill function
    private void Kill() { Destroy(gameObject); }
}
