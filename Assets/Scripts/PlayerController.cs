    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private GameObject focalPoint;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;
    private float powerupStrength = 10f;

    public PowerUpType currentPowerUp = PowerUpType.None;
    public GameObject rocketPrefab;
    public GameObject powerupIndicator;
    public float speed = 5f;
    public bool hasPowerup = false;

    public float hangTime;  
    public float smashSpeed;
    public float explosionForce;
    public float explosionRadius;

    bool smashing = false;
    float floorY;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        playerRigidbody.AddForce(focalPoint.transform.right * horizontalInput * speed);
        playerRigidbody.AddForce(focalPoint.transform.forward * verticalInput * speed);

        powerupIndicator.transform.position = transform.position - new Vector3(0, 0.5f, 0);

        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }

        if (currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !smashing)
        {
            smashing = true;
            StartCoroutine(Smash());
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            hasPowerup = true;
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);

            if (powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }
            powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.Pushback)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            Vector3 awayFromThePlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromThePlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    void LaunchRockets()
    {
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        currentPowerUp = PowerUpType.None;
        powerupIndicator.SetActive(false);
    }

    IEnumerator Smash()
    {
        var enemies = FindObjectsOfType<Enemy>();
        
        floorY = transform.position.y;
        
        float jumpTime = Time.time + hangTime;
        
        while (Time.time < jumpTime)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, smashSpeed);
            yield return null;
        }
        
        while (transform.position.y > floorY)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, -smashSpeed * 2);
            yield return null;
        }
        
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce,
                transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
        }
        
        smashing = false;
    }
}
