using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {


	public int maxHealth = 100;
	public int currentHealth = 100;

	// Use this for initialization
	void Start () {
		
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
		Destroy(gameObject);
		//take away player input if player dies
	}
}
