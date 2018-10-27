using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {


	public GameObject Enemy;

	// Use this for initialization
	void Start () {
		StartCoroutine(spawnEnemies());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator spawnEnemies(){
		while(true){
			Instantiate(Enemy, transform.position, Quaternion.identity);
			yield return new WaitForSeconds(3.0f);
			Debug.Log("EnemySpawned");
		}
		
	}
}
