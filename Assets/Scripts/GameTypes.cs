using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTypes : MonoBehaviour
{

    private static GameObject[] enemySpawners;

    private static int maxNumberOfSpawnedEnemies = 28;
    private static int baseNumberOfEnemies = 5;    // the number of enemies to start with

    public static int totalNumberOfEnemiesThisRound;
    public static int numberOfSpawnedEnemiesThisRound;
    public static int roundKillCount;

    private static int numberOfSpawnedEnemiesCurrently;
    private static int roundNumber = 0;

    public static bool gameStarted;

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            UpdateGameMode();
        }
    }

    public static void StartGameMode(bool shouldStart = true)
    {
        Manager.musicAudioSource.Stop();
        Manager.player.SetActive(true);
        Manager.hudCanvas.SetActive(true);
        var enemiesStillAlive = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemiesStillAlive)
        {
            Destroy(enemy);
        }
        Manager.ResetGame();
        switch (Manager.gameMode)
        {
            case 1:
                StartWaveRespawn();
                break;
            default:
                break;
        }
        GameTypes.gameStarted = shouldStart;
    }

    public void UpdateGameMode()
    {
        switch (Manager.gameMode)
        {
            case 1:
                UpdateWaveRespawn();
                break;
            default:
                break;
        }
    }

    static void StartWaveRespawn()
    {
        ObtainAllSpawners();
        StartNextRound();
    }

    static void UpdateWaveRespawn()
    {
        SpawnReinforcements();
        IsRoundOver();
    }

    static void ObtainAllSpawners()
    {
        enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
    }

    static void SpawnReinforcements()
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

    static int WaveRespawnAlgorithm()
    {
        return baseNumberOfEnemies + (roundNumber * roundNumber) + roundNumber;
    }

    static void IsRoundOver()
    {
        // Debug.Log("Enemies Left: " + (totalNumberOfEnemiesThisRound - roundKillCount));
        if (roundKillCount == totalNumberOfEnemiesThisRound)
        {
            // this means the round has been completed!
            StartNextRound();
        }
    }

    static void StartNextRound()
    {
        Manager.roundsSurvived++;
        roundNumber++;
        Debug.Log("Beginning round " + roundNumber);
        roundKillCount = 0;
        numberOfSpawnedEnemiesThisRound = 0;
        numberOfSpawnedEnemiesCurrently = 0;
        totalNumberOfEnemiesThisRound = WaveRespawnAlgorithm();
    }
}
