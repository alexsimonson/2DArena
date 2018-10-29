﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssistantToTheManager : MonoBehaviour {

	public GameObject grid;
	public GameObject baseLayer;
	public GameObject enemySpawner;
	public GameObject[] enemySpawns;
	private int spawnersLeft;
	private int enemiesLeft = 0;
	private bool gameWon = false;
	public GameObject deathScreen;
	public GameObject winScreen;
	public GameObject mainCam;
	public GameObject player;
	private GameObject player2;

	public bool player2joined = false;

	//we should get references to the UI and change enemies alive and spawners alive
	
	// Use this for initialization
	void Start () {
		grid = GameObject.Find("Grid");
		baseLayer = GameObject.Find("BaseLayer");
		enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawner");
		spawnersLeft = enemySpawns.Length;
		mainCam = GameObject.FindGameObjectWithTag("MainCamera");
		Debug.Log("Game starts with " + spawnersLeft + " spawners.");
		player = GameObject.FindGameObjectWithTag("Player");
		player.GetComponent<PlayerControl>().hasControl = true;
	}

	//based on tag, add or remove to the count tracked by scoreboard
	public void ScoreUpdate(string tagName, bool increase){
		if(tagName=="Enemy"){
			if(increase){
				enemiesLeft++;
//				Debug.Log("Enemy # increased to " + enemiesLeft);
			}else{
				enemiesLeft--;
//				Debug.Log("Enemy # DECREASED to " + enemiesLeft);
			}
		}else if(tagName=="EnemySpawner"){
			if(increase){
				spawnersLeft++;
//				Debug.Log("Enemy spawners # increased to " + spawnersLeft);
			}else{
				spawnersLeft--;
//				Debug.Log("Enemy spawners # DECREASED to " + spawnersLeft);
			}
		}
		
	}

	void Update(){
		CheckWin();
		CheckIfPlayer2Joining();
	}

	void CheckIfPlayer2Joining(){
		if (player2joined == false)
		{
			if(Input.GetButton("Start")){
				Debug.Log("startButtonCalled");
				Add2Player();
			}
		}
	}

	void Add2Player(){
		player2 = Instantiate(player, Vector2.zero, Quaternion.identity);
		player2joined = true;
		player2.GetComponent<PlayerControl>().player1 = false;
	}
	void CheckWin(){
		if(spawnersLeft<=0 && enemiesLeft<=0 && !gameWon){
			gameWon = true;
			//you could just call UI here
			Debug.Log("THE GAME HAS BEEN WON");
			//call function on player to display UI
			//remove control from player
			player.GetComponent<PlayerControl>().hasControl = false;
			Instantiate(winScreen, mainCam.transform.position, Quaternion.identity);	//this pops ui but isn't usable
		}
	}

}