using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{


    public GameObject Enemy;
    public GameObject GameManager;

    // Use this for initialization
    void Start()
    {
        GameManager = GameObject.FindWithTag("GameManager");
        // StartCoroutine(spawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator spawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            GameManager.GetComponent<GameAssistantToTheManager>().ScoreUpdate("Enemy", true);
            yield return new WaitForSeconds(3.0f);
            // Debug.Log("EnemySpawned");
        }

    }

    public void SpawnEnemy()
    {
        Instantiate(Enemy, transform.position, Quaternion.identity);
    }
}
