using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{


    public GameObject Enemy;

    private IEnumerator spawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            Manager.ScoreUpdate("Enemy", true);
            yield return new WaitForSeconds(3.0f);
        }
    }

    public void SpawnEnemy()
    {
        Instantiate(Enemy, transform.position, Quaternion.identity);
    }
}
