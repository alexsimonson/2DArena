using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	// initializing gameobject references
	public GameObject GameAssistantToTheManager;

	// initializing local state variables
	public int maxHealth = 100;
	public int currentHealth = 100;

	// Use this for initialization
	void Start () {
		GameAssistantToTheManager = GameObject.Find("GameAssistantToTheManager");
	}

	public void TakeDamage(int damage){
		currentHealth -= damage;
		CheckDead();
	}

	void CheckDead(){
		if (currentHealth <= 0){
			Death();
		}
	}

	public void Death(){
		GameAssistantToTheManager.GetComponent<GameAssistantToTheManager>().ScoreUpdate(gameObject.tag, false);
		Destroy(gameObject);
		//take away player input if player dies
	}
}
