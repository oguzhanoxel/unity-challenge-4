using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isBoss = false;
    public float spawnInterval;
    private float nextSpawn;
    public int miniEnemySpawnCount;
    private SpawnManager spawnManager;

    public float speed = 3f;
    private GameObject player;
    private Rigidbody enemyRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemyRigidbody = GetComponent<Rigidbody>();

        if (isBoss)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRigidbody.AddForce(lookDirection * speed);

        if (isBoss)
        {
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnInterval;
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
