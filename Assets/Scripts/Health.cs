using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {


	public int maxHealth = 100;
	public int currentHealth = 100;

	public GameObject GameAssistantToTheManager;


	// Use this for initialization
	void Start () {
		GameAssistantToTheManager = GameObject.Find("GameAssistantToTheManager");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage(int damage){
		currentHealth -= damage;
		checkifDead();
	}

	void checkifDead(){
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
