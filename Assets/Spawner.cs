using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject skeletonPrefab;

    [SerializeField]
    private float minimumSpawnTime;

    [SerializeField]
    private float maximumSpawnTime;

    private float timeUntilSpawn;

    public int maxEnemyCount;

    public int spawnRate = 5;

    int maxEnemyEntity = 20;

    int enemyCount;

    void Awake()
    {
        SetTimeUntilSpawn();

    }

    void Update()
    {
        //Find Enemy Count
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        
        //Spawn Duration
        timeUntilSpawn -= Time.deltaTime;

        if (timeUntilSpawn <= 0)
        {
            //Check if it reached max spawn count
            if (enemyCount <= maxEnemyCount-1 && enemyCount <= maxEnemyEntity)
            {
                //Spawn Enemy if not max spawn count
                Instantiate(skeletonPrefab, transform.position, Quaternion.identity);
                SetTimeUntilSpawn();
                maxEnemyCount += spawnRate;
            }
        }
    }


    //Randomized Spawn Timer
    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }
}
