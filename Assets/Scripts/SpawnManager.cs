using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float spawnRange = 9f;
    public GameObject boss;
    public List<GameObject> enemies;
    public List<GameObject> miniEnemies;
    public List<GameObject> powerups;
    public int enemyCount;
    public int waveNumber = 1;
    public int bossRound;

    // Start is called before the first frame update
    void Start()
    {
        int randomPowerupIndex = Random.Range(0, powerups.Count);
        Instantiate(powerups[randomPowerupIndex], GenerateSpawnPosition(), powerups[randomPowerupIndex].transform.rotation);
        SpawnEnemyWave(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        
        if (enemyCount == 0)
        {
            waveNumber++;

            if (waveNumber % bossRound == 0)
            {
                SpawnBossWave(waveNumber);
            }
            else
            {
                SpawnEnemyWave(waveNumber);
            }

            int randomPowerupIndex = Random.Range(0, powerups.Count);
            Instantiate(powerups[randomPowerupIndex], GenerateSpawnPosition(), powerups[randomPowerupIndex].transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 spawnPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return spawnPos;
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemies.Count);
            Instantiate(enemies[randomEnemyIndex], GenerateSpawnPosition(), enemies[randomEnemyIndex].transform.rotation);
        }
    }

    public void SpawnMiniEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomMiniIndex = Random.Range(0, miniEnemies.Count);
            Instantiate(miniEnemies[randomMiniIndex], GenerateSpawnPosition(),
            miniEnemies[randomMiniIndex].transform.rotation);
        }
    }

    void SpawnBossWave(int currentRound)
    {
        int miniEnemysToSpawn;

        if (bossRound != 0)
        {
            miniEnemysToSpawn = currentRound / bossRound;
        }
        else
        {
            miniEnemysToSpawn = 1;
        }

        var _boss = Instantiate(boss, GenerateSpawnPosition(),
        boss.transform.rotation);
        _boss.GetComponent<Enemy>().miniEnemySpawnCount = miniEnemysToSpawn;
    }

}
