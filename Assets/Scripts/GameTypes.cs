using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTypes : MonoBehaviour
{

    private GameObject[] enemySpawners;

    private int maxNumberOfSpawnedEnemies = 28;
    private int baseNumberOfEnemies = 5;    // the number of enemies to start with

    public int totalNumberOfEnemiesThisRound;
    public int numberOfSpawnedEnemiesThisRound;
    public int roundKillCount;

    private int numberOfSpawnedEnemiesCurrently;
    private int roundNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        ObtainAllSpawners();
        StartNextRound();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnReinforcements();
        IsRoundOver();
    }

    void ObtainAllSpawners()
    {
        enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
    }

    void SpawnReinforcements()
    {
        if (numberOfSpawnedEnemiesCurrently < maxNumberOfSpawnedEnemies)
        {
            if (numberOfSpawnedEnemiesThisRound < totalNumberOfEnemiesThisRound)
            {

                // pick random spawn point and spawn
                enemySpawners[Random.Range(0, enemySpawners.Length)].GetComponent<EnemySpawner>().SpawnEnemy();
                numberOfSpawnedEnemiesThisRound++;
                numberOfSpawnedEnemiesCurrently++;
            }
        }
    }

    int WaveRespawnAlgorithm()
    {
        return baseNumberOfEnemies + (roundNumber * roundNumber) + roundNumber;
    }

    void IsRoundOver()
    {
        if (roundKillCount == totalNumberOfEnemiesThisRound)
        {
            // this means the round has been completed!
            StartNextRound();
        }
    }

    void StartNextRound()
    {
        roundNumber++;
        Debug.Log("Beginning round " + roundNumber);
        roundKillCount = 0;
        numberOfSpawnedEnemiesThisRound = 0;
        numberOfSpawnedEnemiesCurrently = 0;
        totalNumberOfEnemiesThisRound = WaveRespawnAlgorithm();
    }
}
