using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssistantToTheManager : MonoBehaviour {

	public GameObject grid;
	public GameObject baseLayer;
	public GameObject enemySpawner;
	public GameObject[] enemySpawns;
	private int spawnersLeft;
	private int enemiesLeft = 0;

	//we should get references to the UI and change enemies alive and spawners alive
	
	// Use this for initialization
	void Start () {
		grid = GameObject.Find("Grid");
		baseLayer = GameObject.Find("BaseLayer");
		enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawner");
		spawnersLeft = enemySpawns.Length;
		Debug.Log("Game starts with " + spawnersLeft + " spawners.");
	}

	//based on tag, add or remove to the count tracked by scoreboard
	public void ScoreUpdate(string tagName, bool increase){
		if(tagName=="Enemy"){
			if(increase){
				enemiesLeft++;
				Debug.Log("Enemy # increased to " + enemiesLeft);
			}else{
				enemiesLeft--;
				Debug.Log("Enemy # DECREASED to " + enemiesLeft);
			}
		}else if(tagName=="EnemySpawner"){
			if(increase){
				spawnersLeft++;
				Debug.Log("Enemy spawners # increased to " + spawnersLeft);
			}else{
				spawnersLeft--;
				Debug.Log("Enemy spawners # DECREASED to " + spawnersLeft);
			}
		}
	}


}
