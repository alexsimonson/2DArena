using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTypes : MonoBehaviour
{

    public static GameObject[] enemySpawners;

    private static int maxNumberOfSpawnedEnemies = 28;
    private static int baseNumberOfEnemies = 5;    // the number of enemies to start with

    public static int totalNumberOfEnemiesThisRound;
    public static int numberOfSpawnedEnemiesThisRound;
    public static int roundKillCount;

    public static int numberOfSpawnedEnemiesCurrently;

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
        GameObject[] enemiesStillAlive = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemiesStillAlive.Length > 0)
        {
            foreach (GameObject enemy in enemiesStillAlive)
            {
                Destroy(enemy);
            }
        }
        Manager.ResetGame();
        GameTypes.gameStarted = shouldStart;
        switch (Manager.gameMode)
        {
            case 1:
                StartWaveRespawn();
                break;
            default:
                break;
        }
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
        StartNextRound(true);
    }

    static void UpdateWaveRespawn()
    {
        SpawnReinforcements();
        // IsRoundOver();
    }

    public static void PrintAllStats(){
        Debug.Log("Rounds Survived: " + Manager.currentRound);
        Debug.Log("number of enemies spawned this round: " + numberOfSpawnedEnemiesThisRound);
        Debug.Log("Round KC: " + roundKillCount);
        Debug.Log("number of enemies spawned currently: " + numberOfSpawnedEnemiesCurrently);
        Debug.Log("Total Enemies This Round: " + totalNumberOfEnemiesThisRound);
    }

    static void ObtainAllSpawners()
    {
        enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
    }

    static void SpawnReinforcements()
    {
        if (enemySpawners.Length > 0)
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
    }

    static int WaveRespawnAlgorithm()
    {
        return baseNumberOfEnemies + (Manager.currentRound * Manager.currentRound) + Manager.currentRound;
    }

    public static void IsRoundOver()
    {
        // Debug.Log("Enemies Left: " + (totalNumberOfEnemiesThisRound - roundKillCount));
        if (roundKillCount == totalNumberOfEnemiesThisRound)
        {
            // this means the round has been completed!
            StartNextRound();
        }
    }

    static void StartNextRound(bool firstRound = false)
    {
        if(!firstRound){
            Manager.currentRound++;
        }
        Debug.Log("Beginning round " + Manager.currentRound);
        roundKillCount = 0;
        numberOfSpawnedEnemiesThisRound = 0;
        numberOfSpawnedEnemiesCurrently = 0;
        totalNumberOfEnemiesThisRound = WaveRespawnAlgorithm();
    }
}
