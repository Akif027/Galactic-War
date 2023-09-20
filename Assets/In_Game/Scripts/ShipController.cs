
using UnityEngine;
using Photon.Pun;


public class ShipController : MonoBehaviourPunCallbacks
{
    public GameObject Cinamacine;
    [Header("Movement")]


    public float rotationSpeed = 180f;
    public float rollSpeed = 60f; // Adjust the roll speed as needed
    public float damping = 0.8f;


    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public float deceleration = 4f;


    private float currentSpeed = 0f;
    private float targetSpeed = 0f;


    [Header("Boost")]

    public float maxBoost = 100f;
    public float currentBoost = 100f;
    public float boostConsumptionRate = 20f; // Rate at which boost decreases over time
    public float boostRechargeRate = 5f; // Rate at which boost recharges over time
    private bool boosting = false;



    [Header("Fire")]
    public Transform[] firePoint;
    public float fireRate = 0.5f;

    public float BulletSpeed = 100;
    public ParticleSystem[] JetFire;
    public GameObject target; // Assign your target GameObject here



    private float nextFireTime = 0f;
    public GameObject bulletPrefab;
    private Rigidbody rb;
    private Objectpool projectilePool;
    private bool isCursorHidden = false;

    private Vector3 velocity;
    PhotonView view;


    float verticalInput;
    public Camera mainCamera;

    public UIManager UIManager;

    private void Awake()
    {
        UIManager = UIManager.instance;
        view = GetComponent<PhotonView>();
        projectilePool = FindObjectOfType<Objectpool>();
    }
    private void Start()
    {
        if (view.IsMine)
        {
            rb = GetComponent<Rigidbody>();

        }


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;



    }

    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            verticalInput = Input.GetAxis("Vertical");

            // Calculate target speed based on vertical input and maxSpeed
            targetSpeed = verticalInput * maxSpeed;

            // Apply acceleration and deceleration
            if (targetSpeed > currentSpeed)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, deceleration * Time.deltaTime);
            }

            //rotate only when moving 

            float rotationInputX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float rotationInputY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            // Apply rotation around y-axis (yaw) and x-axis (pitch)
            transform.Rotate(Vector3.up * rotationInputX);
            transform.Rotate(Vector3.left * rotationInputY);


            if (velocity.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, damping * Time.deltaTime);

                // Calculate roll rotation based on mouse movement
                float rollInput = -Input.GetAxis("Mouse X");
                Quaternion rollRotation = Quaternion.Euler(0f, 0f, rollInput * rollSpeed * Time.deltaTime);
                transform.rotation *= rollRotation;
            }


            float currentMoveSpeed = boosting ? currentSpeed * 2 : currentSpeed;
            Vector3 forwardMovement = transform.forward * currentMoveSpeed * Time.deltaTime;


            // Update velocity with forward movement
            velocity += forwardMovement;
            // Apply velocity to position using rigidbody
            Vector3 newPosition = rb.position + velocity;
            rb.MovePosition(newPosition);

            // Apply damping to velocity
            velocity *= damping;
            SpeedOmeter();

            UpdateBoost();
            if (target != null)
            {

                // Convert the target's world position to screen space
                Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(target.transform.position);

                // Set the crosshair UI position to the target's screen position
                UIManager.instance.crosshair.position = targetScreenPos;
            }

        }

    }


    private void Update()
    {
        //Debug.Log(currentSpeed +"boost "+ boosting);


        if (view.IsMine)
        {

            if (verticalInput > 0)
            {
                for (int i = 0; i < JetFire.Length; i++)
                {
                    JetFire[i].Play();
                    view.RPC("SyncParticleState", RpcTarget.Others, true);
                }

            }
            else
            {
                for (int i = 0; i < JetFire.Length; i++)
                {
                    JetFire[i].Stop();
                    view.RPC("SyncParticleState", RpcTarget.Others, false);
                }
            }


            if (Input.GetKeyDown(KeyCode.Space) && currentBoost >= maxBoost && verticalInput > 0)
            {
                boosting = true;
            }

            if (Input.GetKeyUp(KeyCode.Space) || currentBoost <= 0)
            {
                boosting = false;
            }


            // Rotate based on movement direction and roll based on mouse movement


            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {

                view.RPC("Shoot", RpcTarget.All);

            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isCursorHidden = !isCursorHidden;
                Cursor.visible = isCursorHidden;
                Cursor.lockState = isCursorHidden ? CursorLockMode.None : CursorLockMode.Locked;
            }


        }
        else if (!view.IsMine)
        {
            Cinamacine.SetActive(false);
        }
    }

    private void SpeedOmeter()
    {

        float shipSpeed = velocity.magnitude;
        float displayedSpeed = Mathf.Clamp(shipSpeed * 40, 0f, 100f);
        // Display speed in UI or log it
        UIManager.SpeedMetorText.text = "Speed: " + displayedSpeed.ToString("F0"); // Display speed in UI
                                                                                   // Debug.Log("Speed: " + displayedSpeed.ToString("F0")); // Log speed in console
        UIManager.SpeedMetorSlider.value = displayedSpeed / 100f;
    }

    [PunRPC]
    private void SyncParticleState(bool enable)
    {
        if (enable)
        {

            for (int i = 0; i < JetFire.Length; i++)
            {
                JetFire[i].Play();

            }
        }
        else
        {
            //JetFire.Stop();
            for (int i = 0; i < JetFire.Length; i++)
            {
                JetFire[i].Stop();

            }
        }
    }

    //  [PunRPC]
    private void UpdateBoost()
    {
        float actualBoostConsumptionRate = boosting ? boostConsumptionRate * Time.deltaTime : 0f;

        if (boosting && currentBoost >= actualBoostConsumptionRate)
        {
            currentBoost -= actualBoostConsumptionRate;
        }
        else if (!boosting && currentBoost < maxBoost)
        {
            currentBoost += boostRechargeRate * Time.deltaTime;
        }

        // Stop boosting if currentBoost is less than 10 and not enough for a boost
        if (currentBoost < 10f && currentBoost < actualBoostConsumptionRate)
        {
            boosting = false;
        }

        // Clamp currentBoost within 0 and maxBoost
        currentBoost = Mathf.Clamp(currentBoost, 0f, maxBoost);

        UIManager.boostSlider.value = currentBoost / maxBoost;
        UIManager.boostText.text = Mathf.RoundToInt(currentBoost).ToString() + "/100";
    }


    [PunRPC]
    private void Shoot()
    {
        nextFireTime = Time.time + fireRate;
        // Vector3 Temp = new Vector3(target.transform.position.x, 5, target.transform.position.z);
        for (int i = 0; i < firePoint.Length; i++)
        {
            // Calculate the shooting direction towards the target
            Vector3 shootDirection = (target.transform.position - firePoint[i].position).normalized;

            GameObject projectile = projectilePool.GetpoolObject();
            projectile.GetComponent<Bullet>().SetShooter(gameObject);
            if (projectile != null)
            {
                projectile.transform.position = firePoint[i].position;
                projectile.transform.rotation = Quaternion.LookRotation(shootDirection);
                float playerSpeedThreshold = 1.0f; // You can adjust this threshold as needed
                float bulletBaseSpeed = BulletSpeed; // The base speed of the bullet
                float currentMoveSpeed = boosting ? currentSpeed * 4 : currentSpeed;
                // Calculate the bullet speed based on the player's speed
                float adjustedBulletSpeed = bulletBaseSpeed + Mathf.Max(currentMoveSpeed - playerSpeedThreshold, 0);

                // Apply the adjusted speed to the bullet's velocity
                projectile.GetComponent<Rigidbody>().velocity = shootDirection * adjustedBulletSpeed;
            }
        }

    }
    /*
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Asteroid"))
            {
                if (view.IsMine)
                {

                    PhotonView playerView = other.GetComponent<PhotonView>();
                    if (playerView.IsMine)
                    {
                      //  playerView.RPC("TakeDamage", RpcTarget.AllBuffered, 40);
                    }
                }
            }

    *//*        if (other.gameObject.CompareTag("Bullet") )
            {
                if (view.IsMine)
                {

                    PhotonView playerData = other.gameObject.GetComponent<PhotonView>();
                    PlayerHealth ph = other.GetComponent<PlayerHealth>();
                    if (playerData != null)
                        {
                            playerData.RPC("TakeDamage", RpcTarget.AllBuffered, 40f);
                        if (ph.iSplayerdead)
                        {
                            GameManager.instance.killCount += 1;
                        }

                    }


                }
            }
    *//*

        }*/
}

