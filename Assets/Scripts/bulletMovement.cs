using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMovement : MonoBehaviour {
	Rigidbody2D rb;
	GameObject[] players;
	GameObject player;
	Vector2 targetForward;

	public bool firedByPlayer1 = true;
	// Use this for initialization
	void Start () {

		rb = gameObject.GetComponent<Rigidbody2D>();
		players = GameObject.FindGameObjectsWithTag("Player");
		Debug.Log("There are "+ players.Length + " in the players array");
		if (players[0].GetComponent<PlayerControl>().player1 == false){
			player = players[0];
			Debug.Log("player 0 name " + players[0].gameObject.name);
		}
		else {
			player = players[1];
			Debug.Log("player 1 name " + players[1].gameObject.name);
		}
		targetForward = player.transform.rotation * Vector2.up;


	}
	
	// Update is called once per frame
	void Update () {
		rb.transform.Translate(targetForward*10*Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.tag=="Collider"){
			// Debug.Log("Bullet hit wall");
			Destroy(gameObject);
		}else if(col.tag=="Enemy" || col.tag=="EnemySpawner"){
			// Debug.Log("Bullet hit enemy");
			//this needs to apply damage from the weapon
			col.GetComponent<Health>().TakeDamage(player.GetComponent<PlayerControl>().inHands.damage);
			Destroy(gameObject);
		}
	}
}
