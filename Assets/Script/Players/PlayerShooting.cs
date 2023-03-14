using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Variables
    /////////////
    public int gunIndex;

    private float nextFire;
    private bool recoilWaitOver;

    private Transform cam;
    private GameObject gun;
    private InputSystem inpSys;
    private Transform shootPoint;
    private GunScript gunScript;

    [Header("Components")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject[] mainGun;
    [SerializeField] private AudioSource gunShotSound;

    // References
    //////////////
    private void Awake() { inpSys = new InputSystem(); } // Getting input system

    private void Start()
    {
        // Getting Gun & scripts
        gun = (GameObject)mainGun.GetValue(gunIndex); // Getting gun by the gun index
        gun.SetActive(true); // Enabling the gun
        gunScript = gun.GetComponent<GunScript>();

        // Getting shootPoint
        shootPoint = GameObject.Find($"{gun.gameObject.name} Shoot Point").transform;

        // Getting other components
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform; // Getting camera
    }

    private void FixedUpdate()
    {
        Shoot(); // Shoot function
        RecoilWait(); // Recoil wait function
    }

    // Shoot function
    private void Shoot()
    {
        // If shoot button is pressed
        if (inpSys.Player.Shoot.IsPressed() && Time.time >= nextFire)
        {
            // Getting bullet direction
            Vector3 bulletDirection;

            if (recoilWaitOver)
            {
                float randomX = Random.Range(-gunScript.spread, gunScript.spread); // Getting randomX
                float randomY = Random.Range(-gunScript.spread, gunScript.spread); // Getting randomY

                bulletDirection = new Vector3(randomX, randomY, 1); // Setting recoil
            }
            else { bulletDirection = Vector3.forward; }

            RaycastHit hit; // Making raycast

            // Hitting Raycast
            if (Physics.Raycast(cam.position, cam.TransformDirection(bulletDirection), out hit, Mathf.Infinity))
            {
                // Spawning bullet
                GameObject shootBullet = Instantiate(bullet, shootPoint.position, cam.rotation);
                shootBullet.SetActive(true); // Enabling bullet
                gunShotSound.Play(); // Playing shooting sound

                // Getting bullet script
                Bullet playerBullet = shootBullet.GetComponent<Bullet>();

                playerBullet.targetPoint = hit.point; // Moving bullet at shooting position

                // Giving damage to enemy
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    EnemyLogic enemyScript = hit.collider.gameObject.GetComponentInParent<EnemyLogic>(); // Getting script
                    enemyScript.health -= gunScript.damage; // Giving damage
                }
            }

            // Changing nextFire with fireRate
            nextFire = Time.time + 1f / gunScript.fireRate;
        }
    }

    // RecoilWait function
    private void RecoilWait()
    {
        // If Key is pressed wait for some time and set recoil
        if (inpSys.Player.Shoot.IsPressed())
        {
            Invoke(nameof(OverRecoilWait), gunScript.recoilWaitTime);
        }
        else { recoilWaitOver = false; CancelInvoke(nameof(OverRecoilWait)); } // Else set it to false
    }
    private void OverRecoilWait() { recoilWaitOver = true; }

    // Enabling input system
    private void OnEnable() { inpSys.Player.Enable(); }
    // Disabling input system
    private void OnDisable() { inpSys.Player.Disable(); }
}
