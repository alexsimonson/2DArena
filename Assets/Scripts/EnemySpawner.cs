using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {


	public GameObject Enemy;
	public GameObject GameAssistantToTheManager;

	// Use this for initialization
	void Start () {
		GameAssistantToTheManager = GameObject.Find("GameAssistantToTheManager");
		StartCoroutine(spawnEnemies());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator spawnEnemies(){
		while(true){
			Instantiate(Enemy, transform.position, Quaternion.identity);
			GameAssistantToTheManager.GetComponent<GameAssistantToTheManager>().ScoreUpdate("Enemy", true);
			yield return new WaitForSeconds(3.0f);
			// Debug.Log("EnemySpawned");
		}
		
	}
}
